#Requires -Modules Microsoft.PowerShell.Utility

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
Microsoft.PowerShell.Utility\Set-Alias clear Shell\Clear-Line # was: Microsoft.PowerShell.Core\Clear-Host
Microsoft.PowerShell.Utility\Set-Alias rd Shell\Remove-Directory # was: Microsoft.PowerShell.Management\Remove-Item
Microsoft.PowerShell.Utility\Set-Alias man PSTool\Get-HelpOnline # was: Microsoft.PowerShell.Core\Get-Help
Microsoft.PowerShell.Utility\Set-Alias gp Git\Get-GitRepository @Readonly @Force # was: Microsoft.PowerShell.Management\Get-ItemProperty
Microsoft.PowerShell.Utility\Set-Alias gm Git\Write-GitRepository @Readonly @Force # was: Microsoft.PowerShell.Utility\Get-Member
Microsoft.PowerShell.Utility\New-Alias nv Node\Step-NodePackageVersion @Readonly @Force # was: Microsoft.PowerShell.Utility\New-Variable

# Mask implicit PS alias
Microsoft.PowerShell.Utility\New-Alias verb PSTool\Get-VerbList @Readonly # implicit Microsoft.PowerShell.Utility\Get-Verb

# Mask native PATH executable
Microsoft.PowerShell.Utility\New-Alias clip Microsoft.PowerShell.Management\Set-Clipboard @Readonly # clip.exe
Microsoft.PowerShell.Utility\New-Alias run WindowsSystem\Invoke-CommandPrompt @ReadonlyAllScope # nvm\run.cmd
