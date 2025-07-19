Set-Alias -Name clear -Value Clear-Container # default: Clear-Host
Set-Alias -Name rd -Value Remove-Folder # default: Remove-Item

New-Alias -Name c -Value Set-Location -Option ReadOnly
New-Alias -Name l -Value Get-ChildItem -Option ReadOnly
New-Alias -Name split -Value Split-Path -Option ReadOnly

New-Alias -Name touch -Value New-Item -Option ReadOnly
New-Alias -Name hash -Value Get-FileHash -Option ReadOnly
