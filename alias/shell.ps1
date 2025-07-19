Set-Alias -Name clear -Value Clear-Container # default: Clear-Host
Set-Alias -Name rd -Value Remove-Folder # default: Remove-Item

New-Alias -Name c -Value Set-Location
New-Alias -Name l -Value Get-ChildItem
New-Alias -Name split -Value Split-Path

New-Alias -Name touch -Value New-Item
New-Alias -Name hash -Value Get-FileHash
