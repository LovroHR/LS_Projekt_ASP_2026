# Initialize Logging System for Audio Production Management Project
# This script sets up automatic logging of all prompts/commands into lab-3.

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$logDir = Join-Path $projectRoot '..\lab-3'
$logFile = Join-Path $logDir 'agent_log.txt'

if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

if (-not (Test-Path $logFile)) {
    New-Item -ItemType File -Path $logFile -Force | Out-Null
    Add-Content -Path $logFile -Encoding UTF8 -Value "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') | Agent Logging System Initialized"
}

function Log-Prompt {
    param([string]$Message)
    $timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    Add-Content -Path $logFile -Encoding UTF8 -Value "$timestamp | UserPrompt: $Message" -ErrorAction SilentlyContinue
}

function Log-Tool {
    param([string]$ToolName, [string]$Description)
    $timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    Add-Content -Path $logFile -Encoding UTF8 -Value "$timestamp | Tool: $ToolName | $Description" -ErrorAction SilentlyContinue
}

Write-Host "Audio Production Management - Agent Logging Initialized"
Write-Host "Log file: $logFile"
Write-Host "Available commands: Log-Prompt, Log-Tool"
