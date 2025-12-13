using namespace System.IO

function Get-Directory {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

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

function Get-DirectorySibling {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions('..', 'Directory')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

function Get-DirectoryRelative {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions('..\..', 'Directory')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD | Split-Path | Split-Path) $Path
  }
  Get-ChildItem @FullPath @args
}

function Get-DirectoryHome {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions('~', 'Directory')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Get-ChildItem @FullPath @args
}

function Get-DirectoryCode {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions('~\code', 'Directory')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME\code $Path
  }
  Get-ChildItem @FullPath @args
}

function Get-DirectoryDrive {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions('\', 'Directory')]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Get-ChildItem @FullPath @args
}

New-Alias l Get-Directory
New-Alias l. Get-DirectorySibling
New-Alias l.. Get-DirectoryRelative
New-Alias lh Get-DirectoryHome
New-Alias lc Get-DirectoryCode
New-Alias l/ Get-DirectoryDrive
