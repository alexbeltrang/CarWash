#requires -Version 5.1
param([switch] $VerifyPayload)
$ErrorActionPreference = 'Stop'
$folder = Join-Path ([IO.Path]::GetTempPath()) ('CarWash.SetupTests-' + [guid]::NewGuid().ToString('N'))
$null = New-Item $folder -ItemType Directory
try {
    $selectionPath = Join-Path $folder 'selection.json'
    $selection = [ordered]@{ Desktop = $true; Api = $false; NewDatabase = $false; ApiUrl = 'http://192.168.1.10:5197';
        DatabasePath = (Join-Path $folder 'new.db'); BackupDirectory = ($folder + '-backups'); Usuario = 'fixture-admin'; Clave = 'FixtureOnly42!' }
    function Assert-Selection([bool] $accepted) {
        $selection | ConvertTo-Json | Set-Content $selectionPath -Encoding UTF8
        $failed = $false
        try { & "$PSScriptRoot\configure.ps1" -SelectionPath $selectionPath -InstallDir $folder -Mode Validate | Out-Null }
        catch { $failed = $true }
        if ($failed -eq $accepted) { throw 'Validacion de seleccion no coincide.' }
    }
    Assert-Selection $true
    $selection.NewDatabase = $true; Assert-Selection $false
    $selection.Api = $true; Assert-Selection $true
    $selection.Desktop = $false; Assert-Selection $true
    $selection.NewDatabase = $false; Assert-Selection $false
    [IO.File]::WriteAllText($selection.DatabasePath, 'fixture')
    $hash = (Get-FileHash $selection.DatabasePath).Hash
    Assert-Selection $true
    $selection.NewDatabase = $true; Assert-Selection $false
    if ((Get-FileHash $selection.DatabasePath).Hash -ne $hash) { throw 'Base existente modificada.' }
    $selection.Api = $false; $selection.NewDatabase = $false; Assert-Selection $false
    $selection.Desktop = $true; $selection.ApiUrl = 'https://user:secret@example.invalid'; Assert-Selection $false
    $selection.ApiUrl = 'https://example.invalid'; Assert-Selection $true
    Write-Output 'PASS: 10 combinaciones escritorio/API/base y proteccion de base existente; sin tareas ni instalaciones.'
    $selection.Api = $true
    $backupDestination = $selection.BackupDirectory
    foreach ($unsafeDestination in @($folder, (Join-Path $folder 'backups'), (Join-Path $env:ProgramData 'SplashCar\backups'), [IO.Path]::GetPathRoot($folder), $selection.DatabasePath)) {
        $selection.BackupDirectory = $unsafeDestination
        Assert-Selection $false
    }
    $selection.BackupDirectory = $backupDestination
    $null = New-Item $backupDestination -ItemType Directory
    [IO.File]::WriteAllText((Join-Path $backupDestination 'unrelated.txt'), 'keep')
    Assert-Selection $false
    @{ InstallDir = (Join-Path $folder 'foreign') } | ConvertTo-Json | Set-Content (Join-Path $backupDestination 'backup-owner.json') -Encoding UTF8
    Assert-Selection $false
    @{ InstallDir = $folder } | ConvertTo-Json | Set-Content (Join-Path $backupDestination 'backup-owner.json') -Encoding UTF8
    Assert-Selection $true
    Remove-Item $backupDestination -Recurse -Force
    $junctionDestination = $backupDestination + '-link'
    try {
        $null = New-Item $junctionDestination -ItemType Junction -Target $folder
        $selection.BackupDirectory = $junctionDestination
        Assert-Selection $false
    } finally { if (Test-Path -LiteralPath $junctionDestination) { [IO.Directory]::Delete($junctionDestination) } }
    $selection.Api = $false
    $selection.BackupDirectory = $backupDestination
    Write-Output 'PASS: destinos de respaldo internos/raiz/archivo/enlace/ajenos rechazados antes de instalar; carpeta propia aceptada.'
    $null = New-Item (Join-Path $folder 'Desktop'), (Join-Path $folder 'Templates') -ItemType Directory
    $desktopConfig = Join-Path $PSScriptRoot 'staging\desktop\CarWash.exe.config'
    Copy-Item $desktopConfig (Join-Path $folder 'Desktop\CarWash.exe.config')
    Copy-Item $desktopConfig (Join-Path $folder 'Templates\desktop.config')
    [xml]$config = Get-Content (Join-Path $folder 'Desktop\CarWash.exe.config')
    $entry = $config.CreateElement('add'); $entry.SetAttribute('key', 'InstallerFixture'); $entry.SetAttribute('value', 'keep')
    $null = $config.configuration.appSettings.AppendChild($entry)
    $config.Save((Join-Path $folder 'Desktop\CarWash.exe.config'))
    $selection | ConvertTo-Json | Set-Content $selectionPath -Encoding UTF8
    & "$PSScriptRoot\configure.ps1" -SelectionPath $selectionPath -InstallDir $folder -Mode Install | Out-Null
    [xml]$config = Get-Content (Join-Path $folder 'Desktop\CarWash.exe.config')
    if (($config.configuration.appSettings.add | Where-Object { $_.key -eq 'InstallerFixture' }).value -ne 'keep' -or
        ($config.configuration.appSettings.add | Where-Object { $_.key -eq 'ApiBaseUrl' }).value -ne 'https://example.invalid' -or
        (Test-Path $selectionPath)) { throw 'No se preserva configuracion o no se elimina seleccion temporal.' }
    Write-Output 'PASS: configuracion de escritorio conserva ajustes y elimina seleccion temporal.'
    function Test-Uninstall {
        $cleanupRoot = Join-Path $folder 'cleanup'
        $null = New-Item $cleanupRoot -ItemType Directory
        $fixture = @{ Task = $null; BackupTask = $null; RemovedBackupTask = $false; Processes = @(); Stopped = @(); RemovedTask = $false; RemovedFirewall = $false }
        function Get-ScheduledTask { param($TaskName) if ($TaskName -eq 'CarWashBackup') { $fixture.BackupTask } else { $fixture.Task } }
        function Stop-ScheduledTask { }
        function Unregister-ScheduledTask {
            param($TaskName, $Confirm)
            if ($TaskName -eq 'CarWashBackup') { $fixture.RemovedBackupTask = $true } else { $fixture.RemovedTask = $true }
        }
        function Get-NetFirewallRule { [pscustomobject]@{ DisplayName = 'CarWash API LAN' } }
        function Remove-NetFirewallRule { $fixture.RemovedFirewall = $true }
        function Get-Process { }
        function Stop-Process {
            [CmdletBinding()]
            param($Id, [switch] $Force, [switch] $PassThru)
            $fixture.Stopped += $Id
            if ($Id -eq 222) { Write-Error 'Proceso de prueba ya finalizado.' }
        }
        function Get-CimInstance {
            param($ClassName, $Filter)
            if ($ClassName -eq 'Win32_UserProfile') { [pscustomobject]@{ LocalPath = (Join-Path $cleanupRoot 'profile') } }
            elseif ($ClassName -eq 'Win32_Process') { $fixture.Processes }
            else { throw 'Consulta del sistema no esperada en prueba.' }
        }
        $originalProgramData = $env:ProgramData
        try {
            $env:ProgramData = Join-Path $cleanupRoot 'ProgramData'
            $retainedBackups = Join-Path $cleanupRoot 'retained-backups'
            $null = New-Item $retainedBackups -ItemType Directory
            [IO.File]::WriteAllText((Join-Path $retainedBackups 'copy.db'), 'keep')
            foreach ($case in @('complete', 'desktop', 'foreign-task', 'foreign-backup', 'junction')) {
                $install = Join-Path $cleanupRoot $case
                $shared = Join-Path $cleanupRoot ('shared-' + $case)
                $local = Join-Path $cleanupRoot 'profile\AppData\Local\CarWash'
                $roaming = Join-Path $cleanupRoot 'profile\AppData\Roaming\CarWash'
                $common = Join-Path $env:ProgramData 'SplashCar'
                foreach ($directory in @("$install\Api", "$install\Desktop", "$install\Templates", $shared, $local, $roaming, $common)) {
                    $null = New-Item $directory -ItemType Directory -Force
                    [IO.File]::WriteAllText((Join-Path $directory 'fixture.txt'), 'fixture')
                }
                $databasePath = Join-Path $shared 'client.db'
                foreach ($suffix in @('', '-wal', '-shm', '-journal')) { [IO.File]::WriteAllText(($databasePath + $suffix), 'fixture') }
                $other = Join-Path $shared 'other.db'
                [IO.File]::WriteAllText($other, 'keep')
                [IO.File]::WriteAllText("$install\Desktop\CarWash.exe", 'fixture')
                if ($case -ne 'desktop') { [IO.File]::WriteAllText("$install\Api\Carwash.API.exe", 'fixture') }
                [IO.File]::WriteAllText("$install\unins000.exe", 'fixture')
                $emptyParent = Join-Path $cleanupRoot ('empty-' + $case)
                $emptyChild = Join-Path $emptyParent 'child'
                $null = New-Item $emptyChild -ItemType Directory -Force
                @{ InstallDir = $install; Desktop = $true; Api = ($case -ne 'desktop'); DatabasePath = $databasePath; CreatedDirectories = @($emptyParent, $emptyChild, $shared) } |
                    ConvertTo-Json | Set-Content "$install\installation.json" -Encoding UTF8
                $fixture.Task = $null; $fixture.BackupTask = $null; $fixture.RemovedBackupTask = $false
                $fixture.Stopped = @(); $fixture.RemovedTask = $false; $fixture.RemovedFirewall = $false
                $fixture.Processes = @([pscustomobject]@{ ProcessId = 111; Name = 'CarWash.exe'; ExecutablePath = "$install\Desktop\CarWash.exe"; CommandLine = '' },
                    [pscustomobject]@{ ProcessId = 333; Name = 'CarWash.exe'; ExecutablePath = "$shared\CarWash.exe"; CommandLine = '' })
                if ($case -ne 'desktop') {
                    $owner = $install
                    if ($case -eq 'foreign-task') { $owner = $shared }
                    $fixture.Task = [pscustomobject]@{ Actions = @([pscustomobject]@{ Arguments = '-File "' + $owner + '\Api\run-api.ps1" -InstallDir "' + $owner + '\Api"' }) }
                    $backupOwner = $install
                    if ($case -eq 'foreign-backup') { $backupOwner = $shared }
                    $fixture.BackupTask = [pscustomobject]@{ Actions = @([pscustomobject]@{
                        Execute = "$backupOwner\Api\CarWashSeeder.exe"; Arguments = '--backup "' + $backupOwner + '\backup-settings.json"'
                    }) }
                    $fixture.Processes += [pscustomobject]@{ ProcessId = 222; Name = 'Carwash.API.exe'; ExecutablePath = "$install\Api\Carwash.API.exe"; CommandLine = '' }
                }
                $junction = "$install\Desktop\linked"
                if ($case -eq 'junction') { $null = New-Item $junction -ItemType Junction -Target $shared }
                $failed = $false
                try { & "$PSScriptRoot\uninstall.ps1" -InstallDir $install | Out-Null }
                catch { $failed = $true }
                if ($case -in @('foreign-task', 'foreign-backup', 'junction')) {
                    if (-not $failed -or -not (Test-Path $databasePath) -or $fixture.Stopped.Count -ne 0 -or
                        $fixture.RemovedTask -or $fixture.RemovedBackupTask -or $fixture.RemovedFirewall -or -not (Test-Path "$install\Desktop\fixture.txt")) {
                        throw 'Limpieza debe rechazar otra tarea/enlaces sin efectos laterales.'
                    }
                } else {
                    if ($failed -or (Test-Path "$install\Desktop") -or (Test-Path "$install\Api") -or
                        (Test-Path "$install\Templates") -or (Test-Path "$install\installation.json") -or
                        (Test-Path $local) -or (Test-Path $roaming) -or (Test-Path $emptyParent) -or $fixture.Stopped -notcontains 111 -or
                        $fixture.Stopped -contains 333 -or -not (Test-Path "$install\unins000.exe")) {
                        throw 'Limpieza de carpetas/procesos propios incorrecta.'
                    }
                    if ($case -eq 'complete') {
                        if ((Test-Path $databasePath) -or (Test-Path ($databasePath + '-wal')) -or
                            (Test-Path ($databasePath + '-shm')) -or (Test-Path ($databasePath + '-journal')) -or
                            (Test-Path $common) -or -not $fixture.RemovedTask -or -not $fixture.RemovedBackupTask -or -not $fixture.RemovedFirewall -or
                            $fixture.Stopped -notcontains 222) { throw 'Limpieza API incompleta.' }
                    } elseif (-not (Test-Path $databasePath) -or -not (Test-Path $common) -or
                        $fixture.RemovedTask -or $fixture.RemovedFirewall) { throw 'Escritorio solo no debe tocar API externa.' }
                }
                if ((Get-Content $other -Raw) -ne 'keep') { throw 'Se modificaron archivos ajenos.' }
                if ((Get-Content (Join-Path $retainedBackups 'copy.db') -Raw) -ne 'keep') { throw 'Se eliminaron respaldos externos.' }
                if ($case -eq 'junction') { (Get-Item -LiteralPath $junction -Force).Delete() }
            }
        } finally { $env:ProgramData = $originalProgramData }
        Write-Output 'PASS: limpieza completa/escritorio; base y auxiliares eliminados, archivos ajenos conservados, tarea externa y enlaces rechazados.'
    }
    Test-Uninstall
    if ($VerifyPayload) {
        $staging = Join-Path $PSScriptRoot 'staging'
        foreach ($item in (Get-Content "$staging\payload-manifest.json" -Raw | ConvertFrom-Json)) {
            $path = Join-Path $staging $item.File
            if ($item.File -in @('net481.exe', 'vc_redist.x64.exe')) { $path = Join-Path "$PSScriptRoot\cache" $item.File }
            if ((Get-FileHash $path).Hash -ne $item.SHA256) { throw 'Payload no coincide con su manifiesto.' }
        }
        $database = Join-Path $folder 'api.db'
        Copy-Item "$staging\empty.db" $database
        $seeder = Join-Path $staging 'api\CarWashSeeder.exe'
        & $seeder --verify-empty $database
        if ($LASTEXITCODE -ne 0) { throw 'Base empaquetada no esta vacia.' }
        @{ Usuario = 'fixture-admin'; Clave = 'FixtureOnly42!' } | ConvertTo-Json | Set-Content $selectionPath -Encoding UTF8
        & $seeder $database $selectionPath
        if ($LASTEXITCODE -ne 0 -or (Test-Path $selectionPath)) { throw 'Sembrador no crea administrador o no borra seleccion.' }
        $databaseHash = (Get-FileHash $database).Hash
        & $seeder --verify-schema $database
        if ($LASTEXITCODE -ne 0 -or (Get-FileHash $database).Hash -ne $databaseHash) {
            throw 'Verificacion de base existente fallo o modifico datos.'
        }
        $invalid = Start-Process -FilePath $seeder -ArgumentList @('--verify-schema', ('"' + $selection.DatabasePath + '"')) `
            -NoNewWindow -Wait -PassThru -RedirectStandardError (Join-Path $folder 'invalid.err') `
            -RedirectStandardOutput (Join-Path $folder 'invalid.out')
        try {
            if ($invalid.ExitCode -eq 0 -or (Get-FileHash $selection.DatabasePath).Hash -ne $hash) {
                throw 'No se rechazo la base incompatible o se modifico el archivo.'
            }
        } finally { $invalid.Dispose() }
        Write-Output 'PASS: base existente compatible aceptada e incompatible rechazada; hashes conservados.'
        Copy-Item "$staging\api" (Join-Path $folder 'Api') -Recurse
        Copy-Item "$staging\empty.db" (Join-Path $folder 'empty.db')
        $null = New-Item (Join-Path $folder 'SplashCar') -ItemType Directory
        $systemFixture = @{ Task = $null; Firewall = $null; BackupTask = $null; BackupTrigger = $null; BackupSettings = $null }
        function icacls.exe { $global:LASTEXITCODE = 0 }
        function Get-ScheduledTask { }
        function New-ScheduledTaskAction {
            param($Execute, $WorkingDirectory, $Argument)
            [pscustomobject]@{ Execute = $Execute; WorkingDirectory = $WorkingDirectory; Arguments = $Argument }
        }
        function New-ScheduledTaskTrigger {
            param([switch] $AtStartup, [switch] $Once, $At, $RepetitionInterval)
            [pscustomobject]@{ Interval = $RepetitionInterval; Once = $Once }
        }
        function New-ScheduledTaskPrincipal { [pscustomobject]@{} }
        function New-ScheduledTaskSettingsSet {
            param($ExecutionTimeLimit, [switch] $StartWhenAvailable, [switch] $AllowStartIfOnBatteries, [switch] $DontStopIfGoingOnBatteries, $MultipleInstances)
            [pscustomobject]@{ MultipleInstances = $MultipleInstances }
        }
        function Register-ScheduledTask {
            param($TaskName, $Action, $Trigger, $Principal, $Settings, $Description, [switch] $Force)
            if ($TaskName -eq 'CarWashBackup') {
                $systemFixture.BackupTask = $Action
                $systemFixture.BackupTrigger = $Trigger
                $systemFixture.BackupSettings = $Settings
            } else { $systemFixture.Task = $Action }
        }
        function Get-NetFirewallRule { }
        function New-NetFirewallRule {
            param($DisplayName, $Direction, $Action, $Protocol, $LocalPort, $Profile, $RemoteAddress)
            $systemFixture.Firewall = [pscustomobject]@{ Port = $LocalPort; Profile = $Profile; RemoteAddress = $RemoteAddress }
        }
        function Start-ScheduledTask { throw 'La prueba no debe iniciar una tarea.' }
        $originalProgramData = $env:ProgramData
        try {
            $env:ProgramData = $folder
            foreach ($databaseCase in @('populated', 'new', 'empty')) {
                $newDatabase = $databaseCase -eq 'new'
                $systemFixture.Task = $null
                $systemFixture.Firewall = $null
                $apiFixture = Get-Content (Join-Path $folder 'Api\appsettings.json') -Raw | ConvertFrom-Json
                $apiFixture | Add-Member -NotePropertyName InstallerFixture -NotePropertyValue 'keep' -Force
                $apiFixture | ConvertTo-Json -Depth 20 | Set-Content (Join-Path $folder 'Api\appsettings.json') -Encoding UTF8
                $selection.Desktop = $true; $selection.Api = $true; $selection.NewDatabase = $newDatabase
                $selection.ApiUrl = 'http://localhost:5197'
                $selection.DatabasePath = $database
                if ($newDatabase) { $selection.DatabasePath = Join-Path $folder 'combined-new.db' }
                if ($databaseCase -eq 'empty') {
                    $selection.DatabasePath = Join-Path $folder 'existing-empty.db'
                    Copy-Item "$staging\empty.db" $selection.DatabasePath
                }
                $selection | ConvertTo-Json | Set-Content $selectionPath -Encoding UTF8
                & "$PSScriptRoot\configure.ps1" -SelectionPath $selectionPath -InstallDir $folder -Mode Install -SkipStart | Out-Null
                [xml]$combinedDesktop = Get-Content (Join-Path $folder 'Desktop\CarWash.exe.config')
                $apiFixture = Get-Content (Join-Path $folder 'Api\appsettings.json') -Raw | ConvertFrom-Json
                if (($combinedDesktop.configuration.appSettings.add | Where-Object { $_.key -eq 'ApiBaseUrl' }).value -ne $selection.ApiUrl -or
                    ($combinedDesktop.configuration.appSettings.add | Where-Object { $_.key -eq 'InstallerFixture' }).value -ne 'keep' -or
                    $apiFixture.InstallerFixture -ne 'keep' -or
                    $apiFixture.ConnectionStrings.DefaultConnection -ne ('Data Source="' + $selection.DatabasePath + '"') -or
                    -not (Test-Path $selection.DatabasePath) -or (Test-Path $selectionPath) -or
                    $null -eq $systemFixture.Task -or $systemFixture.Task.Execute -ne 'powershell.exe' -or
                    $null -eq $systemFixture.BackupTask -or $systemFixture.BackupTask.Execute -notlike '*CarWashSeeder.exe' -or
                    $systemFixture.BackupTrigger.Interval.TotalHours -ne 1 -or $systemFixture.BackupSettings.MultipleInstances -ne 'IgnoreNew' -or
                    -not (Test-Path (Join-Path $folder 'backup-settings.json')) -or
                    $null -eq $systemFixture.Firewall -or $systemFixture.Firewall.Port -ne 5197 -or
                    $systemFixture.Firewall.Profile -ne 'Private' -or $systemFixture.Firewall.RemoteAddress -ne 'LocalSubnet' -or
                    (Get-FileHash $database).Hash -ne $databaseHash) {
                    throw 'Fallo configuracion conjunta escritorio/API o conservacion de datos.'
                }
            }
        } finally { $env:ProgramData = $originalProgramData }
        Write-Output 'PASS: configuracion conjunta con base nueva, existente sin usuarios y existente poblada sin cambios; tareas/firewall simulados.'
        $listener = New-Object Net.Sockets.TcpListener([Net.IPAddress]::Loopback, 0)
        $listener.Start(); $port = $listener.LocalEndpoint.Port; $listener.Stop()
        $start = New-Object Diagnostics.ProcessStartInfo
        $start.FileName = Join-Path $staging 'api\Carwash.API.exe'
        $start.WorkingDirectory = Join-Path $staging 'api'
        $start.Arguments = "--urls http://127.0.0.1:$port"
        $start.UseShellExecute = $false
        $start.RedirectStandardOutput = $true; $start.RedirectStandardError = $true
        $start.EnvironmentVariables['ConnectionStrings__DefaultConnection'] = 'Data Source="' + $selection.DatabasePath + '";Pooling=False'
        $start.EnvironmentVariables['ASPNETCORE_ENVIRONMENT'] = 'Production'
        $start.EnvironmentVariables['WhatsApp__Enabled'] = 'false'
        $start.EnvironmentVariables['Printing__Enabled'] = 'false'
        $start.EnvironmentVariables['DOTNET_ROOT'] = $folder
        $process = [Diagnostics.Process]::Start($start)
        try {
            $errorOutput = $process.StandardError.ReadToEndAsync()
            $ready = $false
            $watch = [Diagnostics.Stopwatch]::StartNew()
            while (-not $ready -and $watch.Elapsed.TotalSeconds -lt 30) {
                $line = $process.StandardOutput.ReadLineAsync()
                if (-not $line.Wait(30000) -or $null -eq $line.Result) { throw 'La API empaquetada no inicio.' }
                $ready = $line.Result -match 'Now listening on:'
            }
            if (-not $ready) { throw 'La API no publica su puerto.' }
            $output = $process.StandardOutput.ReadToEndAsync()
            function Protect-Fixture([string] $value) {
                $cipher = [Security.Cryptography.RC2]::Create()
                try {
                    $transform = $cipher.CreateEncryptor([Text.Encoding]::ASCII.GetBytes('C4rW4sh'), [Text.Encoding]::ASCII.GetBytes('20270430'))
                    try {
                        $bytes = [Text.Encoding]::ASCII.GetBytes($value)
                        return [Convert]::ToBase64String($transform.TransformFinalBlock($bytes, 0, $bytes.Length))
                    } finally { $transform.Dispose() }
                } finally { $cipher.Dispose() }
            }
            $body = @{ UserName = (Protect-Fixture 'fixture-admin'); Password = (Protect-Fixture 'FixtureOnly42!') } | ConvertTo-Json
            $login = Invoke-RestMethod "http://127.0.0.1:$port/api/auth/login" -Method Post -ContentType 'application/json' -Body $body -TimeoutSec 10
            if (-not $login.esValido -or -not $login.accessToken) { throw 'No se puede iniciar sesion con el administrador de prueba.' }
            Write-Output 'PASS: API autocontenida inicia sin DOTNET_ROOT instalado y autentica administrador temporal; sin instalaciones/tareas/envios.'
        } finally {
            if (-not $process.HasExited) { $process.Kill(); $process.WaitForExit() }
            $process.Dispose()
        }
    }
} finally {
    Remove-Item $folder -Recurse -Force
    if (Test-Path ($folder + '-backups')) { Remove-Item ($folder + '-backups') -Recurse -Force }
}