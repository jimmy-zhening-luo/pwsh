[hashtable]$Private:Readonly = @{
  Option = 'ReadOnly'
}
[hashtable]$Private:ReadonlyAllScope = @{
  Option = @(
    'ReadOnly'
    'AllScope'
  )
}

#region Reassign
Set-Alias clear Shell\Clear-Line # was: Microsoft.PowerShell.Core\Clear-Host
Set-Alias rd Shell\Remove-Directory # was: Remove-Item
Set-Alias man PSTool\Get-HelpOnline # was: Microsoft.PowerShell.Core\Get-Help
Set-Alias gp Shell\Get-GitRepository @Readonly -Force # was: Get-ItemProperty
Set-Alias gm Shell\Write-GitRepository @Readonly -Force # was: Get-Member
#endregion

#region Mask
# Implicit PowerShell alias
New-Alias verb PSTool\Get-VerbList @Readonly # was: implicit Get-Verb

# PATH executable
New-Alias clip Set-Clipboard @Readonly # was: clip.exe
New-Alias run WindowsSystem\Invoke-CommandPrompt @ReadonlyAllScope # was: nvm\run.cmd
#endregion
