New-Alias l Get-ChildItem

New-Alias l. Get-Sibling
function Get-Sibling {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [ValidateSet([SiblingDirectory])]
    [string]$Path
  )

  Get-ChildItem (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [ValidateSet([RelativeDirectory])]
    [string]$Path
  )

  Get-ChildItem (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias l~ Get-Home
function Get-Home {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [ValidateSet([HomeDirectory])]
    [string]$Path
  )

  Get-ChildItem (Join-Path $HOME $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [ValidateSet([DriveDirectory])]
    [string]$Path
  )

  Get-ChildItem (Join-Path $PWD.Drive.Root $Path) @args
}
