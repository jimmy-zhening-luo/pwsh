New-Alias c Set-Location

New-Alias cc Set-Code
function Set-Code {
  [OutputType([void])]
  param ()

  Set-Location -Path $CODE
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

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
function Set-Drive {
  [OutputType([void])]
  param (
    [PathCompletions("\", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path $PWD.Drive.Root $Path)
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
