#requires -Version 5.1
param([Parameter(Mandatory)] [string] $InstallDir)
$ErrorActionPreference = 'Stop'
$task = Get-ScheduledTask -TaskName 'CarWashApi' -ErrorAction SilentlyContinue
if ($task -and @($task.Actions | Where-Object { $_.Arguments -and $_.Arguments.Contains((Join-Path $InstallDir 'Api')) }).Count -gt 0) {
    Stop-ScheduledTask -TaskName 'CarWashApi'
    $apiExe = Join-Path $InstallDir 'Api\Carwash.API.exe'
    Get-CimInstance Win32_Process -Filter "Name = 'Carwash.API.exe'" |
        Where-Object { $_.ExecutablePath -eq $apiExe } |
        ForEach-Object { Stop-Process -Id $_.ProcessId }
    Unregister-ScheduledTask -TaskName 'CarWashApi' -Confirm:$false
    Get-NetFirewallRule -DisplayName 'CarWash API LAN' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
}