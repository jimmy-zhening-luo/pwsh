using namespace Completer.PathCompleter

function Invoke-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path

  )
  if (-not $env:SSH_CLIENT) {
    if (-not $Path) {
      Invoke-Item -Path $PWD.Path @args
    }
    elseif (Test-Path -Path $Path -PathType Container) {
      Invoke-Item -Path $Path @args
    }
    else {
      throw (Test-Path -Path $Path -PathType Leaf) ? (
        [System.IO.IOException]::new(
          "The path '$Path' is a file, not a directory."
        )
      ) : (
        [System.IO.DirectoryNotFoundException]::new(
          "The directory path '$Path' does not exist."
        )
      )
    }
  }
  else {
    Write-Warning -Message 'Cannot open File Explorer over SSH session'
  }
}

function Invoke-DirectorySibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
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

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
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
