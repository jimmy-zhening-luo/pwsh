New-Alias e Invoke-Directory
function Invoke-Directory {
  [OutputType([void])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )
  if (-not $env:SSH_CLIENT) {
    if (-not $Path) {
      $PSBoundParameters.Path = $PWD
    }

    [hashtable]$Private:Container = @{
      PathType = 'Container'
    }
    if (Test-Path @PSBoundParameters @Container) {
      Invoke-Item @PSBoundParameters @args
    }
  }
}

New-Alias e. Invoke-DirectorySibling
function Invoke-DirectorySibling {
  [OutputType([void])]
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD | Split-Path) $Path
  }
  Invoke-Directory @FullPath @args
}

New-Alias e.. Invoke-DirectoryRelative
function Invoke-DirectoryRelative {
  [OutputType([void])]
  param (
    [PathCompletions('..\..')]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD | Split-Path | Split-Path) $Path
  }
  Invoke-Directory @FullPath @args
}

New-Alias eh Invoke-DirectoryHome
function Invoke-DirectoryHome {
  [OutputType([void])]
  param (
    [PathCompletions('~')]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Invoke-Directory @FullPath @args
}

New-Alias ec Invoke-DirectoryCode
function Invoke-DirectoryCode {
  [OutputType([void])]
  param (
    [PathCompletions('~\code')]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME\code $Path
  }
  Invoke-Directory @FullPath @args
}

New-Alias e/ Invoke-DirectoryDrive
function Invoke-DirectoryDrive {
  [OutputType([void])]
  param (
    [PathCompletions('\')]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Invoke-Directory @FullPath @args
}
