#requires -Version 5.1
param(
    [Parameter(Mandatory)] [string] $SelectionPath,
    [Parameter(Mandatory)] [string] $InstallDir,
    [ValidateSet('Validate', 'Stop', 'Install')] [string] $Mode = 'Install',
    [switch] $SkipStart
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$selection = Get-Content $SelectionPath -Raw -Encoding UTF8 | ConvertFrom-Json

function Test-Selection {
    if (-not $selection.Desktop -and -not $selection.Api) { throw 'Seleccione escritorio o API.' }
    if ($selection.NewDatabase -and -not $selection.Api) { throw 'Base nueva requiere API local.' }
    if ($selection.Desktop) {
        $uri = $null
        if (-not [Uri]::TryCreate($selection.ApiUrl, [UriKind]::Absolute, [ref]$uri) -or
            $uri.Scheme -notin @('http', 'https') -or $uri.UserInfo -or $uri.Query -or $uri.Fragment -or
            $uri.AbsolutePath -ne '/') { throw 'Ingrese una URL base HTTP/HTTPS sin usuario, clave, ruta ni parametros.' }
    }
    if ($selection.Api) {
        $backup = [IO.Path]::GetFullPath($selection.BackupDirectory).TrimEnd('\')
        if ($selection.BackupDirectory -notmatch '^[A-Za-z]:[\\/]' -or $backup.Length -le 3 -or
            $backup -match '\\AppData\\(Local|Roaming)\\CarWash(\\|$)' -or
            [IO.Path]::GetFullPath($selection.DatabasePath).StartsWith($backup + '\', [StringComparison]::OrdinalIgnoreCase)) {
            throw 'Seleccione una carpeta local dedicada de respaldo fuera de las carpetas de datos CarWash.'
        }
        foreach ($managed in @($InstallDir, (Join-Path $env:ProgramData 'SplashCar'))) {
            $managed = [IO.Path]::GetFullPath($managed).TrimEnd('\')
            if ($backup -eq $managed -or $backup.StartsWith($managed + '\', [StringComparison]::OrdinalIgnoreCase) -or
                $managed.StartsWith($backup + '\', [StringComparison]::OrdinalIgnoreCase)) {
                throw 'La carpeta de respaldo no puede contener ni estar dentro de carpetas que se eliminan al desinstalar.'
            }
        }
        $ancestor = $backup
        while ($ancestor) {
            if ((Test-Path -LiteralPath $ancestor) -and
                ((Get-Item -LiteralPath $ancestor -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) {
                throw 'La carpeta de respaldo no puede contener enlaces ni uniones de directorios.'
            }
            $ancestor = Split-Path $ancestor -Parent
        }
        if (Test-Path -LiteralPath $backup) {
            if (-not (Test-Path -LiteralPath $backup -PathType Container)) { throw 'El destino de respaldo debe ser una carpeta.' }
            if (@(Get-ChildItem -LiteralPath $backup -Force).Count -gt 0) {
                $ownerFile = Join-Path $backup 'backup-owner.json'
                if (-not (Test-Path -LiteralPath $ownerFile -PathType Leaf) -or
                    (Get-Content -LiteralPath $ownerFile -Raw -Encoding UTF8 | ConvertFrom-Json).InstallDir -ne [IO.Path]::GetFullPath($InstallDir).TrimEnd('\')) {
                    throw 'La carpeta de respaldo no esta vacia o pertenece a otra instalacion. Elija una carpeta dedicada.'
                }
            }
        }
        if (-not [IO.Path]::IsPathRooted($selection.DatabasePath) -or
            [IO.Path]::GetExtension($selection.DatabasePath) -ne '.db') { throw 'Ingrese una ruta absoluta a un archivo .db.' }
        if ($selection.NewDatabase) {
            if (Test-Path $selection.DatabasePath) { throw 'La base ya existe. Desmarque base nueva para conservarla.' }
        } elseif (-not (Test-Path $selection.DatabasePath -PathType Leaf)) {
            throw 'Seleccione base nueva o indique una base existente compatible.'
        }
        foreach ($value in @($selection.Usuario, $selection.Clave)) {
            if ($value.Length -lt 4 -or $value.Length -gt 64 -or $value -ne $value.Trim() -or
                $value -match '[^\x20-\x7e]') { throw 'Usuario y clave requieren de 4 a 64 caracteres ASCII sin espacios extremos.' }
        }
    }
}

function Get-OwnedTask {
    $task = Get-ScheduledTask -TaskName 'CarWashApi' -ErrorAction SilentlyContinue
    if ($task) {
        $owned = @($task.Actions | Where-Object { $_.Arguments -and $_.Arguments.Contains((Join-Path $InstallDir 'Api')) })
        if ($owned.Count -eq 0) { throw 'CarWashApi ya esta instalada en otra ubicacion. No se modificara esa instalacion.' }
    }
    return $task
}

function Get-OwnedBackupTask {
    $task = Get-ScheduledTask -TaskName 'CarWashBackup' -ErrorAction SilentlyContinue
    if ($task -and @($task.Actions | Where-Object {
        $_.Execute -eq (Join-Path $InstallDir 'Api\CarWashSeeder.exe') -and
        $_.Arguments -eq ('--backup "' + (Join-Path $InstallDir 'backup-settings.json') + '"')
    }).Count -eq 0) { throw 'CarWashBackup pertenece a otra instalacion.' }
    return $task
}

Test-Selection
if ($Mode -eq 'Validate') { Write-Output 'Seleccion valida.'; return }
if ($Mode -eq 'Stop') {
    if ($selection.Api) {
        $backupTask = Get-OwnedBackupTask
        if ($backupTask) { Stop-ScheduledTask -TaskName 'CarWashBackup' }
        $task = Get-OwnedTask
        if ($task) { Stop-ScheduledTask -TaskName 'CarWashApi' }
        $apiExe = Join-Path $InstallDir 'Api\Carwash.API.exe'
        Get-CimInstance Win32_Process -Filter "Name = 'Carwash.API.exe'" |
            Where-Object { $_.ExecutablePath -eq $apiExe } |
            ForEach-Object { Stop-Process -Id $_.ProcessId -ErrorAction Stop }
        if (Get-NetTCPConnection -LocalPort 5197 -State Listen -ErrorAction SilentlyContinue) {
            throw 'El puerto 5197 esta ocupado por otra aplicacion. No se continuara.'
        }
    }
    return
}

try {
    $metadataPath = Join-Path $InstallDir 'installation.json'
    $createdDirectories = @()
    if (Test-Path $metadataPath) {
        $previousInstallation = Get-Content $metadataPath -Raw -Encoding UTF8 | ConvertFrom-Json
        if ($previousInstallation.InstallDir -ne [IO.Path]::GetFullPath($InstallDir).TrimEnd('\')) {
            throw 'El registro de instalacion pertenece a otra ruta.'
        }
        $createdDirectories = @($previousInstallation.CreatedDirectories)
    }
    $installation = [ordered]@{
        InstallDir = [IO.Path]::GetFullPath($InstallDir).TrimEnd('\')
        Desktop = ($selection.Desktop -or (Test-Path (Join-Path $InstallDir 'Desktop\CarWash.exe')))
        Api = ($selection.Api -or (Test-Path (Join-Path $InstallDir 'Api\Carwash.API.exe')))
        DatabasePath = ''
        BackupDirectory = ''
        CreatedDirectories = $createdDirectories
    }
    if ($selection.Api) {
        $installation.DatabasePath = $selection.DatabasePath
        $installation.BackupDirectory = [IO.Path]::GetFullPath($selection.BackupDirectory).TrimEnd('\')
    }
    if ($selection.Api -and -not $selection.NewDatabase) {
        & (Join-Path $InstallDir 'Api\CarWashSeeder.exe') --verify-schema $selection.DatabasePath
        if ($LASTEXITCODE -ne 0) { throw 'La base existente no es compatible; no se iniciara la API.' }
    }
    $installation | ConvertTo-Json -Depth 5 | Set-Content $metadataPath -Encoding UTF8
    if ($selection.Desktop) {
        $path = Join-Path $InstallDir 'Desktop\CarWash.exe.config'
        [xml]$desktopConfig = Get-Content $path
        $entry = @($desktopConfig.configuration.appSettings.add | Where-Object { $_.key -eq 'ApiBaseUrl' })
        if ($entry.Count -ne 1) { throw 'Configuracion del escritorio sin ApiBaseUrl unica.' }
        $entry[0].value = $selection.ApiUrl.TrimEnd('/')
        [xml]$template = Get-Content (Join-Path $InstallDir 'Templates\desktop.config')
        $runtime = $desktopConfig.ImportNode($template.configuration.runtime, $true)
        $null = $desktopConfig.configuration.ReplaceChild($runtime, $desktopConfig.configuration.runtime)
        $desktopConfig.Save($path)
    }
    if ($selection.Api) {
        $apiDir = Join-Path $InstallDir 'Api'
        $null = Get-OwnedTask
        $null = Get-OwnedBackupTask
        foreach ($directory in (@((Join-Path $env:ProgramData 'SplashCar'), (Split-Path $selection.DatabasePath)) | Select-Object -Unique)) {
            if (-not (Test-Path $directory)) {
                $parent = $directory
                while ($parent -and -not (Test-Path $parent)) {
                    $installation.CreatedDirectories += $parent
                    $parent = Split-Path $parent -Parent
                }
                $installation | ConvertTo-Json -Depth 5 | Set-Content $metadataPath -Encoding UTF8
                $null = New-Item $directory -ItemType Directory -Force
                & icacls.exe $directory /inheritance:r /grant:r '*S-1-5-18:(OI)(CI)F' '*S-1-5-32-544:(OI)(CI)F' | Out-Null
                if ($LASTEXITCODE -ne 0) { throw 'No se pudo proteger el directorio de datos.' }
            }
        }
        $path = Join-Path $apiDir 'appsettings.json'
        $apiConfig = Get-Content $path -Raw | ConvertFrom-Json
        $apiConfig.ConnectionStrings.DefaultConnection = 'Data Source="' + $selection.DatabasePath.Replace('"', '""') + '"'
        $apiConfig | ConvertTo-Json -Depth 20 | Set-Content $path -Encoding UTF8
        if ($selection.NewDatabase) {
            [IO.File]::Copy((Join-Path $InstallDir 'empty.db'), $selection.DatabasePath, $false)
        }
        & (Join-Path $apiDir 'CarWashSeeder.exe') $selection.DatabasePath $SelectionPath
        if ($LASTEXITCODE -ne 0) { throw 'No se pudo comprobar o crear el administrador inicial.' }
        $backupDirectory = $installation.BackupDirectory
        $ownerFile = Join-Path $backupDirectory 'backup-owner.json'
        if ((Test-Path -LiteralPath $backupDirectory) -and @(Get-ChildItem -LiteralPath $backupDirectory -Force).Count -gt 0) {
            if (-not (Test-Path -LiteralPath $ownerFile) -or
                (Get-Content -LiteralPath $ownerFile -Raw -Encoding UTF8 | ConvertFrom-Json).InstallDir -ne $installation.InstallDir) {
                throw 'La carpeta de respaldo no esta vacia o pertenece a otra instalacion. Elija una carpeta dedicada.'
            }
        }
        $null = New-Item $backupDirectory -ItemType Directory -Force
        & icacls.exe $backupDirectory /inheritance:r /grant:r '*S-1-5-18:(OI)(CI)F' '*S-1-5-32-544:(OI)(CI)F' | Out-Null
        if ($LASTEXITCODE -ne 0) { throw 'No se pudo proteger la carpeta de respaldo.' }
        @{ InstallDir = $installation.InstallDir } | ConvertTo-Json | Set-Content $ownerFile -Encoding UTF8
        $backupConfiguration = Join-Path $InstallDir 'backup-settings.json'
        @{
            InstallDir = $installation.InstallDir
            DatabasePath = $selection.DatabasePath
            BackupDirectory = $backupDirectory
            ConfigFiles = @((Join-Path $apiDir 'appsettings.json'), (Join-Path $env:ProgramData 'SplashCar\email.json'), (Join-Path $env:ProgramData 'SplashCar\printing.json'))
        } | ConvertTo-Json -Depth 5 | Set-Content $backupConfiguration -Encoding UTF8
        $action = New-ScheduledTaskAction -Execute 'powershell.exe' -WorkingDirectory $apiDir `
            -Argument ('-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File "' + $apiDir + '\run-api.ps1" -InstallDir "' + $apiDir + '" -Port 5197')
        $trigger = New-ScheduledTaskTrigger -AtStartup
        $principal = New-ScheduledTaskPrincipal -UserId 'SYSTEM' -LogonType ServiceAccount -RunLevel Highest
        $settings = New-ScheduledTaskSettingsSet -ExecutionTimeLimit ([TimeSpan]::Zero) -StartWhenAvailable `
            -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
        $null = Register-ScheduledTask -TaskName 'CarWashApi' -Action $action -Trigger $trigger -Principal $principal `
            -Settings $settings -Description 'CarWash API local' -Force
        $backupAction = New-ScheduledTaskAction -Execute (Join-Path $apiDir 'CarWashSeeder.exe') -WorkingDirectory $apiDir `
            -Argument ('--backup "' + $backupConfiguration + '"')
        $backupTrigger = New-ScheduledTaskTrigger -Once -At ([DateTime]::Now.AddMinutes(1)) -RepetitionInterval (New-TimeSpan -Hours 1)
        $backupSettings = New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Hours 1) -StartWhenAvailable `
            -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -MultipleInstances IgnoreNew
        $null = Register-ScheduledTask -TaskName 'CarWashBackup' -Action $backupAction -Trigger $backupTrigger -Principal $principal `
            -Settings $backupSettings -Description 'Respaldo SQLite CarWash cada hora' -Force
        if (-not (Get-NetFirewallRule -DisplayName 'CarWash API LAN' -ErrorAction SilentlyContinue)) {
            $null = New-NetFirewallRule -DisplayName 'CarWash API LAN' -Direction Inbound -Action Allow `
                -Protocol TCP -LocalPort 5197 -Profile Private -RemoteAddress LocalSubnet
        }
        if (-not $SkipStart) {
            Start-ScheduledTask -TaskName 'CarWashApi'
            Start-ScheduledTask -TaskName 'CarWashBackup'
        }
    }
    Write-Output 'Configuracion completada.'
} finally {
    Remove-Item $SelectionPath -Force -ErrorAction SilentlyContinue
}