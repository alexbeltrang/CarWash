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
        if (-not [IO.Path]::IsPathRooted($selection.DatabasePath) -or
            [IO.Path]::GetExtension($selection.DatabasePath) -ne '.db') { throw 'Ingrese una ruta absoluta a un archivo .db.' }
        if ($selection.NewDatabase) {
            if (Test-Path $selection.DatabasePath) { throw 'La base ya existe. Desmarque base nueva para conservarla.' }
            foreach ($value in @($selection.Usuario, $selection.Clave)) {
                if ($value.Length -lt 4 -or $value.Length -gt 64 -or $value -ne $value.Trim() -or
                    $value -match '[^\x20-\x7e]') { throw 'Usuario y clave requieren de 4 a 64 caracteres ASCII sin espacios extremos.' }
            }
        } elseif (-not (Test-Path $selection.DatabasePath -PathType Leaf)) {
            throw 'Seleccione base nueva o indique una base existente compatible.'
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

Test-Selection
if ($Mode -eq 'Validate') { Write-Output 'Seleccion valida.'; return }
if ($Mode -eq 'Stop') {
    if ($selection.Api) {
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
    if ($selection.Api -and -not $selection.NewDatabase) {
        & (Join-Path $InstallDir 'Api\CarWashSeeder.exe') --verify-schema $selection.DatabasePath
        if ($LASTEXITCODE -ne 0) { throw 'La base existente no es compatible; no se iniciara la API.' }
    }
    if ($selection.Desktop) {
        $path = Join-Path $InstallDir 'Desktop\CarWash.exe.config'
        [xml]$config = Get-Content $path
        $entry = @($config.configuration.appSettings.add | Where-Object { $_.key -eq 'ApiBaseUrl' })
        if ($entry.Count -ne 1) { throw 'Configuracion del escritorio sin ApiBaseUrl unica.' }
        $entry[0].value = $selection.ApiUrl.TrimEnd('/')
        [xml]$template = Get-Content (Join-Path $InstallDir 'Templates\desktop.config')
        $runtime = $config.ImportNode($template.configuration.runtime, $true)
        $null = $config.configuration.ReplaceChild($runtime, $config.configuration.runtime)
        $config.Save($path)
    }
    if ($selection.Api) {
        $apiDir = Join-Path $InstallDir 'Api'
        $null = Get-OwnedTask
        foreach ($directory in (@((Join-Path $env:ProgramData 'SplashCar'), (Split-Path $selection.DatabasePath)) | Select-Object -Unique)) {
            if (-not (Test-Path $directory)) {
                $null = New-Item $directory -ItemType Directory -Force
                & icacls.exe $directory /inheritance:r /grant:r '*S-1-5-18:(OI)(CI)F' '*S-1-5-32-544:(OI)(CI)F' | Out-Null
                if ($LASTEXITCODE -ne 0) { throw 'No se pudo proteger el directorio de datos.' }
            }
        }
        $path = Join-Path $apiDir 'appsettings.json'
        $config = Get-Content $path -Raw | ConvertFrom-Json
        $config.ConnectionStrings.DefaultConnection = 'Data Source="' + $selection.DatabasePath.Replace('"', '""') + '"'
        $config | ConvertTo-Json -Depth 20 | Set-Content $path -Encoding UTF8
        if ($selection.NewDatabase) {
            [IO.File]::Copy((Join-Path $InstallDir 'empty.db'), $selection.DatabasePath, $false)
            & (Join-Path $apiDir 'CarWashSeeder.exe') $selection.DatabasePath $SelectionPath
            if ($LASTEXITCODE -ne 0) { throw 'No se pudo crear el administrador inicial.' }
        }
        $action = New-ScheduledTaskAction -Execute 'powershell.exe' -WorkingDirectory $apiDir `
            -Argument ('-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File "' + $apiDir + '\run-api.ps1" -InstallDir "' + $apiDir + '" -Port 5197')
        $trigger = New-ScheduledTaskTrigger -AtStartup
        $principal = New-ScheduledTaskPrincipal -UserId 'SYSTEM' -LogonType ServiceAccount -RunLevel Highest
        $settings = New-ScheduledTaskSettingsSet -ExecutionTimeLimit ([TimeSpan]::Zero) -StartWhenAvailable `
            -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
        $null = Register-ScheduledTask -TaskName 'CarWashApi' -Action $action -Trigger $trigger -Principal $principal `
            -Settings $settings -Description 'CarWash API local' -Force
        if (-not (Get-NetFirewallRule -DisplayName 'CarWash API LAN' -ErrorAction SilentlyContinue)) {
            $null = New-NetFirewallRule -DisplayName 'CarWash API LAN' -Direction Inbound -Action Allow `
                -Protocol TCP -LocalPort 5197 -Profile Private -RemoteAddress LocalSubnet
        }
        if (-not $SkipStart) { Start-ScheduledTask -TaskName 'CarWashApi' }
    }
    Write-Output 'Configuracion completada.'
} finally {
    Remove-Item $SelectionPath -Force -ErrorAction SilentlyContinue
}