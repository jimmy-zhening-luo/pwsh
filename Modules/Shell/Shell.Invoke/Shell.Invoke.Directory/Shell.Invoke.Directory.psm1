New-Alias e Shell\Invoke-Directory
function Invoke-Directory {
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($env:SSH_CLIENT) {
    return Get-File @PSBoundParameters
  }

  if ($Path) {
    $IsFile = @{
      PathType = 'Leaf'
    }
    if (Test-Path @PSBoundParameters @IsFile) {
      Invoke-Workspace @PSBoundParameters
    }
    else {
      [void](Invoke-Item @PSBoundParameters)
    }
  }
  else {
    [void](Invoke-Item -Path $PWD)
  }
}

New-Alias e. Shell\Invoke-DirectorySibling
function Invoke-DirectorySibling {
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path ($PWD | Split-Path) $Path
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
    Path = Join-Path ($PWD | Split-Path | Split-Path) $Path
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
    Path = Join-Path $HOME $Path
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
    Path = Join-Path $HOME\code $Path
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
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Invoke-Directory @FullPath @args
}
