New-Alias c Set-Directory
function Set-Directory {
  [OutputType([void])]
  param (
    [PathCompletions(".", "Directory")]
    [string]$Path
  )

  if ($Path) {
    Set-Location -Path $Path @args
  }
  else {
    Set-Location @args
  }
}

New-Alias c. Set-Sibling
function Set-Sibling {
  [OutputType([void])]
  param (
    [PathCompletions("..", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path $PWD.Path) $Path)
}

New-Alias c.. Set-Relative
function Set-Relative {
  [OutputType([void])]
  param (
    [PathCompletions("..\..", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path)
}

New-Alias c~ Set-Home
function Set-Home {
  [OutputType([void])]
  param (
    [PathCompletions("~", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path $HOME $Path)
}

New-Alias cc Set-Code
function Set-Code {
  [OutputType([void])]
  param (
    [PathCompletions("~\code", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path "~\code" $Path)
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
function Set-Drive {
  [OutputType([void])]
  param (
    [PathCompletions("\", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path "\" $Path)
}

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
function Set-DriveD {
  [OutputType([void])]
  param (
    [PathCompletions("D:\", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path 'D:\' $Path)
}
