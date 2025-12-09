New-Alias l Shell\Get-Directory
function Get-Directory {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
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
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias l.. Shell\Get-DirectoryRelative
function Get-DirectoryRelative {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location | Split-Path | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias l~ Shell\Get-DirectoryHome
function Get-DirectoryHome {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
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
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $HOME\code $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias l/ Shell\Get-DirectoryDrive
function Get-DirectoryDrive {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location).Drive.Root $Path
  }
  Get-ChildItem @FullPath @args
}
