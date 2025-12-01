New-Alias e Shell\Invoke-Directory
function Invoke-Directory {
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($env:SSH_CLIENT) {
    Get-File @PSBoundParameters
  }
  else {
    if ($Path) {
      if (Test-Path @PSBoundParameters -PathType Leaf) {
        Invoke-Workspace @PSBoundParameters
      }
      else {
        Invoke-Item @PSBoundParameters
      }
    }
    else {
      Invoke-Item -Path $PWD.Path
    }
  }
}

New-Alias e. Shell\Invoke-DirectorySibling
function Invoke-DirectorySibling {
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias e.. Shell\Invoke-DirectoryRelative
function Invoke-DirectoryRelative {
  param (
    [PathCompletions('..\..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
  }

  Invoke-Directory @FullPath @args

}

New-Alias e~ Shell\Invoke-DirectoryHome
function Invoke-DirectoryHome {
  param (
    [PathCompletions('~')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~' $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias ec Shell\Invoke-DirectoryCode
function Invoke-DirectoryCode {
  param (
    [PathCompletions('~\code')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~\code' $Path
  }

  Invoke-Directory @FullPath @args
}

New-Alias e/ Shell\Invoke-DirectoryDrive
function Invoke-DirectoryDrive {
  param (
    [PathCompletions('\')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '\' $Path
  }

  Invoke-Directory @FullPath @args
}
