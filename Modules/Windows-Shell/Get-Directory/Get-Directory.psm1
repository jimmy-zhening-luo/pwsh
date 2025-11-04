New-Alias l Get-ChildItem

New-Alias l. Get-Sibling
function Get-Sibling {
  param (
    [ValidateSet([SiblingDirectory])]
    [string]$Path
  )
  Get-ChildItem (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  param (
    [ValidateSet([RelativeDirectory])]
    [string]$Path
  )
  Get-ChildItem (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias l~ Get-Home
function Get-Home {
  param (
    [ValidateSet([HomeDirectory])]
    [string]$Path
  )
  Get-ChildItem (Join-Path $HOME $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  param (
    [ValidateSet([DriveDirectory])]
    [string]$Path
  )
  Get-ChildItem (Join-Path $PWD.Drive.Root $Path) @args
}
