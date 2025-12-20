using namespace Completer.PathCompleter

function Get-Directory {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  if ($Path) {
    Get-ChildItem -Path $Path @args
  }
  else {
    Get-ChildItem @args
  }
}

function Get-DirectorySibling {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

function Get-DirectoryRelative {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path ($PWD.Path | Split-Path | Split-Path) $Path) @args
}

function Get-DirectoryHome {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [PathLocationCompletions(
      '~',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $HOME $Path) @args
}

function Get-DirectoryCode {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $REPO_ROOT $Path) @args
}

function Get-DirectoryDrive {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [PathLocationCompletions(
      '\',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $PWD.Drive.Root $Path) @args
}

New-Alias l Get-Directory
New-Alias l. Get-DirectorySibling
New-Alias l.. Get-DirectoryRelative
New-Alias lh Get-DirectoryHome
New-Alias lc Get-DirectoryCode
New-Alias l/ Get-DirectoryDrive
