using namespace System.IO

New-Alias l Shell\Get-Directory
function Get-Directory {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Get-ChildItem @PSBoundParameters @args
  }
  else {
    Get-ChildItem @args
  }
}

New-Alias l. Shell\Get-DirectorySibling
function Get-DirectorySibling {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l.. Shell\Get-DirectoryRelative
function Get-DirectoryRelative {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l~ Shell\Get-DirectoryHome
function Get-DirectoryHome {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $HOME $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias lc Shell\Get-DirectoryCode
function Get-DirectoryCode {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path "$HOME\code" $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l/ Shell\Get-DirectoryDrive
function Get-DirectoryDrive {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '\' $Path
  }

  Get-ChildItem @FullPath @args
}
