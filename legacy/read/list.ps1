New-Alias l Get-ChildItem

New-Alias l. Get-SiblingItem
New-Alias l.. Get-SiblingItem
New-Alias ls. Get-SiblingItem
New-Alias ls.. Get-SiblingItem
function Get-SiblingItem {
  Get-ChildItem -Path .. @args
}

New-Alias l~ Get-HomeItem
New-Alias ls~ Get-HomeItem
function Get-HomeItem {
  Get-ChildItem -Path ~ @args
}
