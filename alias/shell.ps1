Set-Alias clear Clear-Container # default: Clear-Host
Set-Alias rd Remove-Folder # default: Remove-Item

New-Alias -Option ReadOnly c Set-Location
New-Alias -Option ReadOnly l Get-ChildItem
New-Alias -Option ReadOnly split Split-Path

New-Alias -Option ReadOnly touch New-Item
New-Alias -Option ReadOnly t New-Item
New-Alias -Option ReadOnly mk mkdir
New-Alias -Option ReadOnly hash Get-FileHash
