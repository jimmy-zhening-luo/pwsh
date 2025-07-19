New-Alias -Name l. -Value Get-SiblingItem -Option ReadOnly
New-Alias -Name l.. -Value Get-SiblingItem -Option ReadOnly
New-Alias -Name ls. -Value Get-SiblingItem -Option ReadOnly
New-Alias -Name ls.. -Value Get-SiblingItem -Option ReadOnly
function Get-SiblingItem {
  Get-ChildItem -Path .. @args
}

New-Alias -Name l~ -Value Get-HomeItem -Option ReadOnly
New-Alias -Name ls~ -Value Get-HomeItem -Option ReadOnly
function Get-HomeItem {
  Get-ChildItem -Path $HOME @args
}
