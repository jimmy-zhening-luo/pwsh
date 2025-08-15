New-Alias l Get-ChildItem

New-Alias l. Get-Sibling
function Get-Sibling {
  param (
    [ValidateSet([SiblingFolder])]
    [System.String]$Path
  )
  Get-ChildItem -Path (Join-Path (Split-Path -Parent $PWD) $Path) @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  param (
    [ValidateSet([RelativeFolder])]
    [System.String]$Path
  )
  Get-ChildItem -Path (Join-Path (Split-Path -Parent (Split-Path -Parent $PWD)) $Path) @args
}

New-Alias l~ Get-Home
function Get-Home {
  param (
    [ValidateSet([HomeFolder])]
    [System.String]$Path
  )
  Get-ChildItem -Path (Join-Path $HOME $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  param (
    [ValidateSet([DriveFolder])]
    [System.String]$Path
  )
  Get-ChildItem -Path (Join-Path $PWD.Drive.Root $Path) @args
}
