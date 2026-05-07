# Agent Logging Initialization Script
# Osiguraj da agent_log.txt datoteka postoji

$projectRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$workspaceRoot = Split-Path -Parent $projectRoot
$logDir = Join-Path $workspaceRoot "lab-3"
$logFile = Join-Path $logDir "agent_log.txt"

# Kreiraj datoteku ako ne postoji
if (-not (Test-Path $logDir)) {
    New-Item -Path $logDir -ItemType Directory -Force | Out-Null
}

if (-not (Test-Path $logFile)) {
    New-Item -Path $logFile -ItemType File -Force | Out-Null
    Write-Host "Agent log file initialized: $logFile"
}

# Zapisi inicijalizaciju
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Add-Content -Path $logFile -Value "$timestamp | Agent logging initialized"
Add-Content -Path $logFile -Value ""
