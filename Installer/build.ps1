#requires -Version 5.1
param(
    [string] $MSBuildPath,
    [switch] $StageDesktopOnly,
    [switch] $StagePayloadOnly
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Import-Module (Join-Path $PSHOME 'Modules\Microsoft.PowerShell.Security\Microsoft.PowerShell.Security.psd1') -ErrorAction Stop
Import-Module (Join-Path $PSHOME 'Modules\Microsoft.PowerShell.Utility\Microsoft.PowerShell.Utility.psd1') -ErrorAction Stop
$root = Split-Path $PSScriptRoot
$staging = Join-Path $PSScriptRoot 'staging'
if (-not $MSBuildPath -or -not (Test-Path $MSBuildPath)) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    $MSBuildPath = & $vswhere -latest -requires Microsoft.Component.MSBuild -find 'MSBuild/**/Bin/MSBuild.exe' |
        Where-Object { $_ -notmatch 'amd64' } | Select-Object -First 1
}
if (-not $MSBuildPath -or -not (Test-Path $MSBuildPath)) { throw 'Falta MSBuild de Visual Studio.' }
$desktop = Join-Path $staging 'desktop'
if (Test-Path $desktop) { Remove-Item $desktop -Recurse -Force }
& $MSBuildPath (Join-Path $root 'CarWash.csproj') /t:Build /p:Configuration=Release /p:Platform=x64 `
    "/p:OutputPath=$desktop\" "/p:IntermediateOutputPath=$staging\obj\desktop\" /verbosity:quiet
if ($LASTEXITCODE -ne 0) { throw 'No se pudo compilar el escritorio.' }
foreach ($file in @('CarWash.exe', 'CarWash.exe.config', 'e_sqlite3.dll')) {
    if (-not (Test-Path (Join-Path $desktop $file))) { throw "Falta archivo del escritorio: $file" }
}
if ($StageDesktopOnly) { Write-Host 'Escritorio preparado en staging/desktop.'; return }
[xml]$desktopConfig = Get-Content "$desktop\CarWash.exe.config"
($desktopConfig.configuration.appSettings.add | Where-Object { $_.key -eq 'NameLDB' }).value = '%LOCALAPPDATA%\CarWash\CarWash.db'
($desktopConfig.configuration.appSettings.add | Where-Object { $_.key -eq 'ApiIgnorarSSL' }).value = 'false'
$desktopConfig.Save("$desktop\CarWash.exe.config")
$mobile = [IO.Path]::GetFullPath((Join-Path $root '..\CarWash.Mobile'))
$apiProject = [IO.Path]::GetFullPath((Join-Path $root '..\CarWashApi\Carwash.API.csproj'))
$api = Join-Path $staging 'api'
if (Test-Path $api) { Remove-Item $api -Recurse -Force }
dotnet publish $apiProject -c Release -r win-x64 --self-contained true -o $api `
    --artifacts-path "$staging\obj\api" --verbosity quiet
if ($LASTEXITCODE -ne 0) { throw 'No se pudo publicar la API autocontenida.' }
dotnet publish "$mobile\installer\seeder\CarWashSeeder.csproj" -c Release -r win-x64 --self-contained true -o $api `
    --artifacts-path "$staging\obj\seeder" --verbosity quiet
if ($LASTEXITCODE -ne 0) { throw 'No se pudo publicar el sembrador.' }
if (Test-Path "$api\publish") { Remove-Item "$api\publish" -Recurse -Force }
Get-ChildItem $api -Recurse -File | Where-Object {
    ($_.Extension -eq '.json' -and $_.Name -notmatch '\.(deps|runtimeconfig)\.json$') -or
    $_.Name -match '\.(db|sqlite|sqlite3)(-wal|-shm|-journal)?$' -or $_.Extension -in @('.pfx', '.p12', '.key', '.pem')
} | Remove-Item -Force
$safeConfig = [ordered]@{
    ConnectionStrings = @{ DefaultConnection = 'Data Source=CarWash.db' }
    Logging = @{ LogLevel = @{ Default = 'Information'; 'Microsoft.AspNetCore' = 'Warning' } }
    Email = @{ Host = ''; Port = 587; Username = ''; Password = ''; FromAddress = ''; FromName = 'CarWash'; EnableSsl = $true }
    WhatsApp = @{ Enabled = $false }
    Printing = @{ Enabled = $false; PrinterName = '' }
    AllowedHosts = '*'
}
$safeConfig | ConvertTo-Json -Depth 10 | Set-Content "$api\appsettings.json" -Encoding UTF8
Copy-Item "$mobile\installer\run-api.ps1" "$api\run-api.ps1"
$schema = Join-Path $staging 'empty.db'
if (Test-Path $schema) { Remove-Item $schema -Force }
& "$api\CarWashSeeder.exe" --create-schema $schema "$mobile\docs\database\carwash-sqlite.sql"
if ($LASTEXITCODE -ne 0) { throw 'No se pudo generar la base vacia.' }
& "$api\CarWashSeeder.exe" --verify-empty $schema
if ($LASTEXITCODE -ne 0) { throw 'La base contiene registros.' }
$cache = Join-Path $PSScriptRoot 'cache'
$null = New-Item $cache -ItemType Directory -Force
[Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
function Get-MicrosoftInstaller([string] $name, [string] $url) {
    $path = Join-Path $cache $name
    if (-not (Test-Path $path)) {
        $partial = "$path.partial"
        Invoke-WebRequest $url -UseBasicParsing -OutFile $partial -MaximumRedirection 10
        Move-Item $partial $path -Force
    }
    $signature = Get-AuthenticodeSignature $path
    if ($signature.Status -ne 'Valid' -or $signature.SignerCertificate.Subject -notmatch 'Microsoft Corporation') {
        throw "Firma Microsoft no valida: $name. Revise la descarga en cache."
    }
}
Get-MicrosoftInstaller 'net481.exe' 'https://go.microsoft.com/fwlink/?linkid=2203305'
Get-MicrosoftInstaller 'vc_redist.x64.exe' 'https://aka.ms/vs/17/release/vc_redist.x64.exe'
$runtimeFiles = @(Get-ChildItem $desktop -File | Where-Object { $_.Extension -in @('.exe', '.dll', '.config') }) +
    @(Get-ChildItem $api -Recurse -File | Where-Object { $_.Extension -ne '.pdb' })
$manifest = @($runtimeFiles | ForEach-Object {
    [ordered]@{ File = $_.FullName.Substring($staging.Length + 1); SHA256 = (Get-FileHash $_.FullName).Hash }
})
$manifest += @(Get-Item $schema, "$cache\net481.exe", "$cache\vc_redist.x64.exe" | ForEach-Object {
    [ordered]@{ File = $_.Name; SHA256 = (Get-FileHash $_.FullName).Hash }
})
$manifest | ConvertTo-Json -Depth 4 | Set-Content "$staging\payload-manifest.json" -Encoding UTF8
if ($StagePayloadOnly) { Write-Host 'Payload offline preparado y firmas verificadas.'; return }
$iscc = @("$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe", "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe") |
    Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $iscc) { throw 'Instale Inno Setup 6 en el equipo generador; no se necesita en destino.' }
& $iscc "$PSScriptRoot\CarWash.iss"
if ($LASTEXITCODE -ne 0) { throw 'No se pudo compilar el instalador.' }
Write-Host "Instalador: $PSScriptRoot\output\CarWash-Setup-x64.exe"