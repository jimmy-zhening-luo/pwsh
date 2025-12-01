using namespace System.IO

New-Alias l Get-Directory
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

New-Alias l. Get-DirectorySibling
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

New-Alias l.. Get-DirectoryRelative
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

New-Alias l~ Get-DirectoryHome
function Get-DirectoryHome {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~' $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias lc Get-DirectoryCode
function Get-DirectoryCode {
  [OutputType([DirectoryInfo[]], [FileInfo[]])]
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~\code' $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l/ Get-DirectoryDrive
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
