param(
    [string]$EventName = 'UnknownEvent'
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..\..')
$logFile = Join-Path $workspaceRoot 'lab-3\agent_log.txt'
$logDir = Split-Path -Parent $logFile

if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

if (-not (Test-Path $logFile)) {
    New-Item -ItemType File -Path $logFile -Force | Out-Null
    Add-Content -Path $logFile -Encoding UTF8 -Value "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') | Agent Logging System Initialized"
}

$payload = [Console]::In.ReadToEnd()

if ([string]::IsNullOrWhiteSpace($payload)) {
    $payload = $env:CopilotHookInput
}

if ([string]::IsNullOrWhiteSpace($payload)) {
    $payload = $env:Input
}

if ([string]::IsNullOrWhiteSpace($payload)) {
    $payload = '[no payload captured]'
}

$timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$safePayload = $payload.Trim()

Add-Content -Path $logFile -Encoding UTF8 -Value "$timestamp | ${EventName}: $safePayload"