New-Alias l Get-Directory
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
function Get-Home {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("~", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path "~" $Path) @args
}

New-Alias lc Get-Code
function Get-Code {
  param (
    [PathCompletions("~\code", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path "~\code" $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  [OutputType(
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param (
    [PathCompletions("\", "Directory")]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path "\" $Path) @args
}
