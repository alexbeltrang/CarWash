#requires -Version 5.1
param([switch] $VerifyPayload)
$ErrorActionPreference = 'Stop'
$folder = Join-Path ([IO.Path]::GetTempPath()) ('CarWash.SetupTests-' + [guid]::NewGuid().ToString('N'))
$null = New-Item $folder -ItemType Directory
try {
    $selectionPath = Join-Path $folder 'selection.json'
    $selection = [ordered]@{ Desktop = $true; Api = $false; NewDatabase = $false; ApiUrl = 'http://192.168.1.10:5197';
        DatabasePath = (Join-Path $folder 'new.db'); Usuario = 'fixture-admin'; Clave = 'FixtureOnly42!' }
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
        $listener = New-Object Net.Sockets.TcpListener([Net.IPAddress]::Loopback, 0)
        $listener.Start(); $port = $listener.LocalEndpoint.Port; $listener.Stop()
        $start = New-Object Diagnostics.ProcessStartInfo
        $start.FileName = Join-Path $staging 'api\Carwash.API.exe'
        $start.WorkingDirectory = Join-Path $staging 'api'
        $start.Arguments = "--urls http://127.0.0.1:$port"
        $start.UseShellExecute = $false
        $start.RedirectStandardOutput = $true; $start.RedirectStandardError = $true
        $start.EnvironmentVariables['ConnectionStrings__DefaultConnection'] = 'Data Source="' + $database + '";Pooling=False'
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
} finally { Remove-Item $folder -Recurse -Force }