New-Alias l Get-Directory
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Directory {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions(".", "Directory")]
    [string]$Path
  )

  if ($Path) {
    Get-ChildItem -Path $Path @args
  }
  else {
    Get-ChildItem @args
  }
}

New-Alias l. Get-Sibling
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Sibling {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("..", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias l.. Get-Relative
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Relative {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("..\..", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias l~ Get-Home
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Home {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("~", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $HOME $Path) @args
}

New-Alias lc Get-Code
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Code {
  param (
    [PathCompletions("~\code", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $CODE $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Function
#>
function Get-Drive {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("\", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $PWD.Drive.Root $Path) @args
}
