# Initialize Logging System for Audio Production Management Project
# This script sets up automatic logging of all prompts/commands

$projectRoot = Get-Location
$logDir = Join-Path $projectRoot '.github' 'hooks'
$logFile = Join-Path $logDir 'agent_log.txt'

# Ensure log directory exists
if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

# Initialize log file if it doesn't exist
if (-not (Test-Path $logFile)) {
    New-Item -ItemType File -Path $logFile -Force | Out-Null
    Add-Content -Path $logFile -Value "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') | Agent Logging System Initialized"
}

# Create a function to log prompts
function Log-Prompt {
    param([string]$Message)
    $timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    Add-Content -Path $logFile -Value "$timestamp | UserPrompt: $Message" -ErrorAction SilentlyContinue
}

# Create a function to log tool usage
function Log-Tool {
    param([string]$ToolName, [string]$Description)
    $timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    Add-Content -Path $logFile -Value "$timestamp | Tool: $ToolName | $Description" -ErrorAction SilentlyContinue
}

Write-Host "Audio Production Management - Agent Logging Initialized"
Write-Host "Log file: $logFile"
Write-Host "Available commands: Log-Prompt, Log-Tool"
