using namespace Completer.PathCompleter

function Set-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  if ($Path) {
    Set-Location -Path $Path @args
  }
  elseif ($args) {
    Set-Location @args
  }
  else {
    Set-Location -Path (Split-Path $PWD.Path)
  }
}

function Set-DirectorySibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return Split-Path $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

function Set-DirectoryRelative {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path ($PWD.Path | Split-Path | Split-Path) $Path) @args
}

function Set-DirectoryHome {

  [OutputType([void])]
  param(

    [PathCompletions(
      '~',
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path $HOME $Path) @args
}

function Set-DirectoryCode {

  [OutputType([void])]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path $REPO_ROOT $Path) @args
}

function Set-Drive {

  [OutputType([void])]
  param(

    [PathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path $PWD.Drive.Root $Path) @args
}

function Set-DriveD {

  [OutputType([void])]
  param(

    [PathCompletions(
      'D:',
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Set-Location -Path (Join-Path D: $Path) @args
}

New-Alias c Set-Directory
New-Alias c. Set-DirectorySibling
New-Alias c.. Set-DirectoryRelative
New-Alias ch Set-DirectoryHome
New-Alias cc Set-DirectoryCode
New-Alias c/ Set-Drive
New-Alias d/ Set-DriveD
