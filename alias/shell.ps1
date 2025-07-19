Set-Alias -Name clear -Value Clear-Container # default: Clear-Host
Set-Alias -Name rd -Value Remove-Folder # default: Remove-Item

New-Alias -Option ReadOnly -Name c -Value Set-Location
New-Alias -Option ReadOnly -Name l -Value Get-ChildItem
New-Alias -Option ReadOnly -Name split -Value Split-Path

New-Alias -Option ReadOnly -Name touch -Value New-Item
New-Alias -Option ReadOnly -Name hash -Value Get-FileHash
