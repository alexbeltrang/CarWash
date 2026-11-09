#requires -Version 5.1
param([Parameter(Mandatory)] [string] $InstallDir)
$ErrorActionPreference = 'Stop'

function Assert-NoLink([string] $Path) {
    $current = [IO.Path]::GetFullPath($Path)
    while ($current) {
        if (Test-Path -LiteralPath $current) {
            if ((Get-Item -LiteralPath $current -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) {
                throw 'No se borran rutas que contienen enlaces o uniones de directorios.'
            }
        }
        $parent = Split-Path $current -Parent
        if ($parent -eq $current) { break }
        $current = $parent
    }
}

function Assert-Tree([string] $Path) {
    Assert-NoLink $Path
    if (Test-Path -LiteralPath $Path -PathType Container) {
        foreach ($child in Get-ChildItem -LiteralPath $Path -Force) { Assert-Tree $child.FullName }
    }
}

function Remove-OwnedTree([string] $Path) {
    Assert-NoLink $Path
    if (-not (Test-Path -LiteralPath $Path)) { return }
    if (Test-Path -LiteralPath $Path -PathType Container) {
        foreach ($child in Get-ChildItem -LiteralPath $Path -Force) { Remove-OwnedTree $child.FullName }
        [IO.Directory]::Delete($Path, $false)
    } else {
        Remove-Item -LiteralPath $Path -Force
    }
}

$InstallDir = [IO.Path]::GetFullPath($InstallDir).TrimEnd('\')
if (-not (Split-Path $InstallDir -Parent) -or
    $InstallDir -eq $env:ProgramFiles -or $InstallDir -eq ${env:ProgramFiles(x86)} -or
    $InstallDir -eq $env:windir -or $InstallDir -eq $env:ProgramData -or $InstallDir -eq $env:USERPROFILE) {
    throw 'La ruta no es una carpeta dedicada de instalacion.'
}
Assert-NoLink $InstallDir
$metadataPath = Join-Path $InstallDir 'installation.json'
$hasDesktop = Test-Path (Join-Path $InstallDir 'Desktop\CarWash.exe')
$hasApi = Test-Path (Join-Path $InstallDir 'Api\Carwash.API.exe')
$database = $null
$createdDirectories = @()
if (Test-Path -LiteralPath $metadataPath) {
    $metadata = Get-Content -LiteralPath $metadataPath -Raw -Encoding UTF8 | ConvertFrom-Json
    if ($metadata.InstallDir -ne $InstallDir) { throw 'El registro de instalacion pertenece a otra ruta.' }
    $hasDesktop = $hasDesktop -or $metadata.Desktop
    $hasApi = $hasApi -or $metadata.Api
    $database = $metadata.DatabasePath
    $createdDirectories = @($metadata.CreatedDirectories)
} elseif (-not (Test-Path (Join-Path $InstallDir 'payload-manifest.json'))) {
    throw 'No se identifica esta carpeta como una instalacion CarWash.'
}
$apiDir = Join-Path $InstallDir 'Api'
$apiExe = Join-Path $apiDir 'Carwash.API.exe'
$task = $null
$backupTask = $null
if ($hasApi) {
    $backupTask = Get-ScheduledTask -TaskName 'CarWashBackup' -ErrorAction SilentlyContinue
    if ($backupTask -and @($backupTask.Actions | Where-Object {
        $_.Execute -eq (Join-Path $apiDir 'CarWashSeeder.exe') -and
        $_.Arguments -eq ('--backup "' + (Join-Path $InstallDir 'backup-settings.json') + '"')
    }).Count -eq 0) { throw 'Otra instalacion usa CarWashBackup; no se modificara su tarea.' }
    $task = Get-ScheduledTask -TaskName 'CarWashApi' -ErrorAction SilentlyContinue
    if ($task -and @($task.Actions | Where-Object {
        $_.Arguments -match ('-File\s+"' + [regex]::Escape((Join-Path $apiDir 'run-api.ps1')) + '"(?:\s|$)')
    }).Count -eq 0) { throw 'Otra instalacion usa CarWashApi; no se borraran sus datos ni su tarea.' }
    if (-not $database -and (Test-Path (Join-Path $apiDir 'appsettings.json'))) {
        $apiConfig = Get-Content (Join-Path $apiDir 'appsettings.json') -Raw -Encoding UTF8 | ConvertFrom-Json
        Add-Type -AssemblyName System.Data
        $connection = New-Object System.Data.Common.DbConnectionStringBuilder
        $connection.ConnectionString = $apiConfig.ConnectionStrings.DefaultConnection
        $database = [string]$connection['Data Source']
        if (-not [IO.Path]::IsPathRooted($database)) { $database = Join-Path $apiDir $database }
    }
}

$trees = @('Desktop', 'Api', 'Templates' | ForEach-Object { Join-Path $InstallDir $_ })
if ($hasApi) { $trees += Join-Path $env:ProgramData 'SplashCar' }
if ($hasDesktop) {
    foreach ($profile in Get-CimInstance Win32_UserProfile -Filter 'Special = FALSE') {
        if ($profile.LocalPath) {
            $trees += Join-Path $profile.LocalPath 'AppData\Local\CarWash'
            $trees += Join-Path $profile.LocalPath 'AppData\Roaming\CarWash'
        }
    }
}
$trees = @($trees | Select-Object -Unique)
$files = @('installation.json', 'payload-manifest.json', 'empty.db', 'configure.ps1', 'uninstall.ps1', 'configuration-error.txt', 'backup-settings.json' |
    ForEach-Object { Join-Path $InstallDir $_ })
if ($hasApi -and $database) {
    if (-not [IO.Path]::IsPathRooted($database) -or [IO.Path]::GetExtension($database) -ne '.db') {
        throw 'La base registrada no es una ruta absoluta .db.'
    }
    $files += @('', '-wal', '-shm', '-journal' | ForEach-Object { $database + $_ })
}
foreach ($tree in $trees) { Assert-Tree $tree }
foreach ($file in $files) { Assert-NoLink $file }
foreach ($directory in $createdDirectories) { if ($directory) { Assert-NoLink $directory } }
$processes = @(Get-CimInstance Win32_Process)
if ($hasApi -and @($processes | Where-Object {
    $_.Name -eq 'Carwash.API.exe' -and $_.ExecutablePath -ne $apiExe
}).Count -gt 0) { throw 'Hay otra API CarWash activa; revise datos compartidos antes de desinstalar.' }
if ($backupTask) { Stop-ScheduledTask -TaskName 'CarWashBackup' }
if ($task) { Stop-ScheduledTask -TaskName 'CarWashApi' }
foreach ($process in ($processes | Sort-Object @{ Expression = { if ($_.Name -match '^(powershell|pwsh)\.exe$') { 0 } else { 1 } } })) {
    if ($process.ExecutablePath -eq (Join-Path $InstallDir 'Desktop\CarWash.exe') -or
        ($hasApi -and ($process.ExecutablePath -eq $apiExe -or
        ($process.ExecutablePath -eq (Join-Path $apiDir 'CarWashSeeder.exe') -and
         $process.CommandLine -match [regex]::Escape((Join-Path $InstallDir 'backup-settings.json'))) -or
        ($process.Name -match '^(powershell|pwsh)\.exe$' -and
         $process.CommandLine -match ('-File\s+"' + [regex]::Escape((Join-Path $apiDir 'run-api.ps1')) + '"(?:\s|$)'))))) {
        $stopErrors = @()
        $stopped = Stop-Process -Id $process.ProcessId -Force -PassThru -ErrorAction SilentlyContinue -ErrorVariable stopErrors
        if ($stopErrors -and (Get-Process -Id $process.ProcessId -ErrorAction SilentlyContinue)) {
            throw 'No se pudo detener un proceso CarWash; no se borraran archivos en uso.'
        }
        if ($stopped -and -not $stopped.WaitForExit(10000)) { throw 'Un proceso CarWash no termino; no se borraran archivos en uso.' }
    }
}
if ($task) { Unregister-ScheduledTask -TaskName 'CarWashApi' -Confirm:$false }
if ($backupTask) { Unregister-ScheduledTask -TaskName 'CarWashBackup' -Confirm:$false }
if ($hasApi) {
    Get-NetFirewallRule -DisplayName 'CarWash API LAN' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
}
foreach ($file in $files) {
    if (Test-Path -LiteralPath $file) { Remove-Item -LiteralPath $file -Force }
}
foreach ($tree in $trees) { Remove-OwnedTree $tree }
foreach ($directory in ($createdDirectories | Where-Object { $_ } | Sort-Object Length -Descending)) {
    if ((Test-Path -LiteralPath $directory -PathType Container) -and
        @(Get-ChildItem -LiteralPath $directory -Force).Count -eq 0) {
        [IO.Directory]::Delete($directory, $false)
    }
}
Write-Output 'Limpieza de datos CarWash completada.'