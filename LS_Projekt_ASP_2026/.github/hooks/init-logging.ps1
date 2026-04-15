# Agent Logging Initialization Script
# Osiguraj da agent_log.txt datoteka postoji

$logFile = ".github/hooks/agent_log.txt"

# Kreiraj datoteku ako ne postoji
if (-not (Test-Path $logFile)) {
    New-Item -Path $logFile -ItemType File -Force | Out-Null
    Write-Host "Agent log file initialized: $logFile"
}

# Zapisi inicijalizaciju
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
Add-Content -Path $logFile -Value "$timestamp | Agent logging initialized"
Add-Content -Path $logFile -Value ""
