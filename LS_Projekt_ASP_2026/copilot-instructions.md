# Copilot Instructions for Agent Logging

You are working in a project that requires logging of all user prompts to `.github/hooks/agent_log.txt` for audit and debugging purposes.

## CRITICAL INSTRUCTION: Log Every User Prompt

**BEFORE you start working on ANY task**, you MUST log the user's prompt to the agent log file.

### How to Log:
1. The logging system is **AUTO-INITIALIZED** when you open a PowerShell terminal in VS Code
2. Use the `Log-Prompt` function in your terminal:
   ```powershell
   Log-Prompt "Your message here"
   ```

3. Or use the traditional method:
   ```powershell
   $timestamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
   Add-Content -Path '.github/hooks/agent_log.txt' -Value "$timestamp | UserPrompt: Your message"
   ```

### Example:
- User says: "Add a for loop"
- You run: `Log-Prompt "Add a for loop"`
- Result: `2026-04-15 20:25:10 | UserPrompt: Add a for loop`

### Important Notes:
- This MUST happen for EVERY user input in the conversation
- Log-Prompt function is automatically available in PowerShell terminals
- The log file will be created automatically if it doesn't exist
- Always append, never overwrite (use `Add-Content`)

## Auto-Initialization Details

- **File**: `.vscode/init-logging.ps1`
- **Trigger**: Automatically runs when any PowerShell terminal opens in VS Code
- **Functions available**:
  - `Log-Prompt "message"` - Logs user prompts
  - `Log-Tool "toolname" "description"` - Logs tool usage

## Logging Conventions

- **Format**: `YYYY-MM-DD HH:mm:ss | UserPrompt: [text]`
- **When**: At the very beginning of handling each request
- **Location**: `.github/hooks/agent_log.txt`
- **Character encoding**: UTF-8

This logging helps track agent activities and user interactions for project management.
