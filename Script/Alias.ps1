#region Override
# was: Clear-Host
Set-Alias clear Shell\Clear-Line

# was: Remove-Item
Set-Alias rd Shell\Remove-Directory

# was: Get-Help
Set-Alias man PSHelp\Get-HelpOnline

# was: Get-ItemProperty
Set-Alias gp Code\Get-GitRepository -Force -Option ReadOnly

# was: Get-Member
Set-Alias gm Code\Write-GitRepository -Force -Option ReadOnly

#endregion


#region Implicit
# was: Get-Verb (implicit)
New-Alias verb PSHelp\Get-VerbList -Option ReadOnly

#endregion


#region Path
# was: clip.exe
New-Alias clip Set-Clipboard -Option ReadOnly

# was: nvm\run.cmd
New-Alias run WindowsSystem\Invoke-CommandPrompt -Option ReadOnly, AllScope

#endregion
