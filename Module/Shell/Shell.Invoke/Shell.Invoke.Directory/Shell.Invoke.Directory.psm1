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

function Invoke-DirectorySibling {

  [OutputType([void])]

  param(

    [PathCompletions('..')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }
  Invoke-Directory @FullPath @args
}

function Invoke-DirectoryRelative {

  [OutputType([void])]

  param(

    [PathCompletions('..\..')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  }
  Invoke-Directory @FullPath @args
}

function Invoke-DirectoryHome {

  [OutputType([void])]

  param(

    [PathCompletions('~')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Invoke-Directory @FullPath @args
}

function Invoke-DirectoryCode {

  [OutputType([void])]

  param(

    [PathCompletions('~\code')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME code $Path
  }
  Invoke-Directory @FullPath @args
}

function Invoke-DirectoryDrive {

  [OutputType([void])]

  param(

    [PathCompletions('\')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Invoke-Directory @FullPath @args
}

New-Alias e Invoke-Directory
New-Alias e. Invoke-DirectorySibling
New-Alias e.. Invoke-DirectoryRelative
New-Alias eh Invoke-DirectoryHome
New-Alias ec Invoke-DirectoryCode
New-Alias e/ Invoke-DirectoryDrive
