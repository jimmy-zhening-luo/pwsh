New-Alias explore Invoke-Directory
New-Alias e Invoke-Directory
function Invoke-Directory {
  param([string]$Path)

  if (-not $env:SSH_CLIENT) {

    if ($Path) {
      if (Test-Path $Path -PathType Leaf) {
        Edit-File $Path @args
      }
      else {
        Invoke-Item $Path @args
      }
    }
    else {
      Invoke-Item $PWD.Path @args
    }
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path
  )

  Invoke-Directory (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path
  )

  Invoke-Directory (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path
  )

  Invoke-Directory (Join-Path $HOME $Path) @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [PathCompletions("\", "")]
    [string]$Path
  )

  Invoke-Directory (Join-Path $PWD.Drive.Root $Path) @args
}
