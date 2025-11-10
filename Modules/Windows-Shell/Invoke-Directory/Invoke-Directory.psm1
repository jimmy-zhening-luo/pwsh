New-Alias e Invoke-Directory
function Invoke-Directory {
  param(
    [PathCompletions(".")]
    [string]$Path
  )

  if ($env:SSH_CLIENT) {
    Read-Item -Path $Path
  }
  else {
    if ($Path) {
      if (Test-Path -Path $Path -PathType Leaf) {
        Edit-Item -Path $Path
      }
      else {
        Invoke-Item -Path $Path
      }
    }
    else {
      Invoke-Item -Path "."
    }
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [PathCompletions("..")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path (Split-Path $PWD.Path) $Path)
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [PathCompletions("..\..")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path)
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [PathCompletions("~")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path "~" $Path)
}

New-Alias ec Invoke-Code
function Invoke-Code {
  param (
    [PathCompletions("~\code")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path "~\code" $Path)
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [PathCompletions("\")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path "\" $Path)
}
