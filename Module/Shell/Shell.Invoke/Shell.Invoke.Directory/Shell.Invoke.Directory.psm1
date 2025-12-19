using namespace Completer.PathCompleter

function Invoke-Directory {

  [OutputType([void])]

  param(

    [PathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path

  )
  if (-not $env:SSH_CLIENT) {
    if (-not $Path) {
      $PSBoundParameters.Path = $PWD
    }

    [hashtable]$Private:Container = @{
      PathType = 'Container'
    }
    if (Test-Path @PSBoundParameters @Private:Container) {
      Invoke-Item @PSBoundParameters @args
    }
  }
}

function Invoke-DirectorySibling {

  [OutputType([void])]

  param(

    [PathLocationCompletions(
      '..',
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }
  Invoke-Directory @Private:FullPath @args
}

function Invoke-DirectoryRelative {

  [OutputType([void])]

  param(

    [PathLocationCompletions(
      '..\..',
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  }
  Invoke-Directory @Private:FullPath @args
}

function Invoke-DirectoryHome {

  [OutputType([void])]

  param(

    [PathLocationCompletions(
      '~',
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Invoke-Directory @Private:FullPath @args
}

function Invoke-DirectoryCode {

  [OutputType([void])]

  param(

    [PathLocationCompletions(
      '~\code',
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $REPO_ROOT $Path
  }
  Invoke-Directory @Private:FullPath @args
}

function Invoke-DirectoryDrive {

  [OutputType([void])]

  param(

    [PathLocationCompletions(
      '\',
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Invoke-Directory @Private:FullPath @args
}

New-Alias e Invoke-Directory
New-Alias e. Invoke-DirectorySibling
New-Alias e.. Invoke-DirectoryRelative
New-Alias eh Invoke-DirectoryHome
New-Alias ec Invoke-DirectoryCode
New-Alias e/ Invoke-DirectoryDrive
