<#
.SYNOPSIS
Runs a command in the Windows Command Prompt, 'cmd.exe'.

.DESCRIPTION
Starts a new instance of the command interpreter, 'cmd.exe' to carry out the supplied command (along with any flags) before exiting the command processor.

.COMPONENT
WindowsSystem

.LINK
https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
#>
function Invoke-CommandPrompt {

  & $env:ComSpec /c @args

  if ($LASTEXITCODE -notin 0, 1) {
    throw "cmd.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

New-Alias restart Restart-Computer
New-Alias sesv Set-Service
New-Alias remsv Remove-Service
