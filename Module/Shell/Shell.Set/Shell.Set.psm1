using namespace Completer.PathCompleter

function Set-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  if ($Path -or $args) {
    Set-Location @PSBoundParameters @args
  }
  else {
    Set-Location -Path (Split-Path $PWD.Path)
  }
}

function Set-DirectorySibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }
  Set-Location @Private:FullPath @args
}

function Set-DirectoryRelative {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  }
  Set-Location @Private:FullPath @args
}

function Set-DirectoryHome {

  [OutputType([void])]
  param(

    [PathLocationCompletions(
      '~',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Set-Location @Private:FullPath @args
}

function Set-DirectoryCode {

  [OutputType([void])]
  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $REPO_ROOT $Path
  }
  Set-Location @Private:FullPath @args
}

function Set-Drive {

  [OutputType([void])]
  param(

    [PathLocationCompletions(
      '\',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Set-Location @Private:FullPath @args
}

function Set-DriveD {

  [OutputType([void])]
  param(

    [PathLocationCompletions(
      'D:',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path D: $Path
  }
  Set-Location @Private:FullPath @args
}

New-Alias c Set-Directory
New-Alias c. Set-DirectorySibling
New-Alias c.. Set-DirectoryRelative
New-Alias ch Set-DirectoryHome
New-Alias cc Set-DirectoryCode
New-Alias c/ Set-Drive
New-Alias d/ Set-DriveD
