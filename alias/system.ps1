New-Alias -Option AllScope, ReadOnly -Name run -Value Invoke-CommandPrompt

New-Alias -Option ReadOnly -Name restart -Value Restart-Computer

New-Alias -Option ReadOnly -Name sesv -Value Set-Service
New-Alias -Option ReadOnly -Name remsv -Value Remove-Service

New-Alias -Option ReadOnly -Name gapx -Value Get-AppxPackage
New-Alias -Option ReadOnly -Name remapx -Value Remove-AppxPackage
New-Alias -Option ReadOnly -Name wg -Value winget
