New-Alias l Get-ChildItem

New-Alias l. Get-Sibling
function Get-Sibling {
  param (
    [ValidateSet([SiblingFolder])]
    [string]$Path
  )
  Get-ChildItem (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  param (
    [ValidateSet([RelativeFolder])]
    [string]$Path
  )
  Get-ChildItem (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias l~ Get-Home
function Get-Home {
  param (
    [ValidateSet([HomeFolder])]
    [string]$Path
  )
  Get-ChildItem (Join-Path $HOME $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  param (
    [ValidateSet([DriveFolder])]
    [string]$Path
  )
  Get-ChildItem (Join-Path $PWD.Drive.Root $Path) @args
}
