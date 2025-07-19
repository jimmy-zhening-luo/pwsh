New-Alias -Option ReadOnly -Name l. -Value Get-SiblingItem
New-Alias -Option ReadOnly -Name l.. -Value Get-SiblingItem
New-Alias -Option ReadOnly -Name ls. -Value Get-SiblingItem
New-Alias -Option ReadOnly -Name ls.. -Value Get-SiblingItem
function Get-SiblingItem {
  Get-ChildItem -Path .. @args
}

New-Alias -Option ReadOnly -Name l~ -Value Get-HomeItem
New-Alias -Option ReadOnly -Name ls~ -Value Get-HomeItem
function Get-HomeItem {
  Get-ChildItem -Path $HOME @args
}
