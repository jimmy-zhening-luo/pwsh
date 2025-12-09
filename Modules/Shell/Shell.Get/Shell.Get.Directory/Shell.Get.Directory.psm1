New-Alias l Get-Directory
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

New-Alias l. Get-DirectorySibling
function Get-DirectorySibling {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path ($PWD | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias l.. Get-DirectoryRelative
function Get-DirectoryRelative {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path ($PWD | Split-Path | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias lh Get-DirectoryHome
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

New-Alias lc Get-DirectoryCode
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

New-Alias l/ Get-DirectoryDrive
function Get-DirectoryDrive {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Get-ChildItem @FullPath @args
}
