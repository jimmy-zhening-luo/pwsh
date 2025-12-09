$Force = @{
  Force = $True
}
$Readonly = @{
  Option = 'ReadOnly'
}
$ReadonlyAllScope = @{
  Option = 'ReadOnly', 'AllScope'
}

# Reassign PS alias
Set-Alias clear Shell\Clear-Line # was: Microsoft.PowerShell.Core\Clear-Host
Set-Alias rd Shell\Remove-Directory # was: Remove-Item
Set-Alias man PSTool\Get-HelpOnline # was: Microsoft.PowerShell.Core\Get-Help
Set-Alias gp Git\Get-GitRepository @Readonly @Force # was: Get-ItemProperty
Set-Alias gm Git\Write-GitRepository @Readonly @Force # was: Get-Member
New-Alias nv Node\Step-NodePackageVersion @Readonly @Force # was: New-Variable

# Mask implicit PS alias
New-Alias verb PSTool\Get-VerbList @Readonly # implicit Get-Verb

# Mask native PATH executable
New-Alias clip Set-Clipboard @Readonly # clip.exe
New-Alias run WindowsSystem\Invoke-CommandPrompt @ReadonlyAllScope # nvm\run.cmd
