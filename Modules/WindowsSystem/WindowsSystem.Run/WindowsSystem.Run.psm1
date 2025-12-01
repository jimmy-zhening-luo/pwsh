<#
.SYNOPSIS
Runs a command in the Windows Command Prompt, 'cmd.exe'.
.DESCRIPTION
Starts a new instance of the command interpreter, 'cmd.exe' to carry out the supplied command (along with any flags) before exiting the command processor.
.LINK
https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
#>
function Invoke-CommandPrompt {
  & cmd /c $args
}
