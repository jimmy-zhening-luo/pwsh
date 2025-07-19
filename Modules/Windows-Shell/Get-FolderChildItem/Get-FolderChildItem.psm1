New-Alias -Name l. -Value Get-SiblingItem
New-Alias -Name l.. -Value Get-SiblingItem
New-Alias -Name ls. -Value Get-SiblingItem
New-Alias -Name ls.. -Value Get-SiblingItem
function Get-SiblingItem {
  Get-ChildItem -Path .. @args
}

New-Alias -Name l~ -Value Get-HomeItem
New-Alias -Name ls~ -Value Get-HomeItem
function Get-HomeItem {
  Get-ChildItem -Path $HOME @args
}
