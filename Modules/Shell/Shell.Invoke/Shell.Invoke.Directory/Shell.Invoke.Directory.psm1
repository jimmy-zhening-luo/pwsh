New-Alias e Shell\Invoke-Directory
function Invoke-Directory {
  [OutputType([void], [string[]])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if (-not $Path) {
    $PSBoundParameters.Path = $PWD
  }

  if ($env:SSH_CLIENT) {
    return Shell\Get-File @PSBoundParameters
  }

  $Container = @{
    PathType = 'Container'
  }
  if (Test-Path @PSBoundParameters @Container) {
    Invoke-Item @PSBoundParameters
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
  Shell\Invoke-Directory @FullPath @args
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
  Shell\Invoke-Directory @FullPath @args
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
  Shell\Invoke-Directory @FullPath @args
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
  Shell\Invoke-Directory @FullPath @args
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
  Shell\Invoke-Directory @FullPath @args
}
