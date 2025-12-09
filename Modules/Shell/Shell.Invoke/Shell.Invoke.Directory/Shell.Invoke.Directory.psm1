New-Alias e Invoke-Directory
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
    return Get-File @PSBoundParameters
  }

  $Container = @{
    PathType = 'Container'
  }
  if (Test-Path @PSBoundParameters @Container) {
    Invoke-Item @PSBoundParameters
  }
}

New-Alias e. Invoke-DirectorySibling
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

New-Alias e.. Invoke-DirectoryRelative
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

New-Alias e~ Invoke-DirectoryHome
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

New-Alias ec Invoke-DirectoryCode
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

New-Alias e/ Invoke-DirectoryDrive
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
