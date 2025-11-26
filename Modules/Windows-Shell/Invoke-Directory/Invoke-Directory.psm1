New-Alias e Invoke-Directory
function Invoke-Directory {
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($env:SSH_CLIENT) {
    Read-Item @PSBoundParameters
  }
  else {
    if ($Path) {
      if (Test-Path @PSBoundParameters -PathType Leaf) {
        # Edit-ShellItem @PSBoundParameters
      }
      else {
        Invoke-Item @PSBoundParameters
      }
    }
    else {
      Invoke-Item -Path '.'
    }
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [PathCompletions('..\..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
  }

  Invoke-Directory @FullPath @args

}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [PathCompletions('~')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~' $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias ec Invoke-Code
function Invoke-Code {
  param (
    [PathCompletions('~\code')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~\code' $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [PathCompletions('\')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '\' $Path
  }

  Invoke-Directory @FullPath @args
}
