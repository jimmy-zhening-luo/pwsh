New-Alias -Name restart -Value Restart-Computer -Option ReadOnly

New-Alias -Name sesv -Value Set-Service -Option ReadOnly
New-Alias -Name remsv -Value Remove-Service -Option ReadOnly

New-Alias -Name gapx -Value Get-AppxPackage -Option ReadOnly
New-Alias -Name remapx -Value Remove-AppxPackage -Option ReadOnly
New-Alias -Name wg -Value winget -Option ReadOnly
