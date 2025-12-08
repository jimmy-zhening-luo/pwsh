using namespace System.IO

Microsoft.PowerShell.Utility\New-Alias l Shell\Get-Directory
function Get-Directory {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Microsoft.PowerShell.Management\Get-ChildItem @PSBoundParameters @args
  }
  else {
    Microsoft.PowerShell.Management\Get-ChildItem @args
  }
}

Microsoft.PowerShell.Utility\New-Alias l. Shell\Get-DirectorySibling
function Get-DirectorySibling {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Microsoft.PowerShell.Management\Get-ChildItem @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias l.. Shell\Get-DirectoryRelative
function Get-DirectoryRelative {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Microsoft.PowerShell.Management\Get-ChildItem @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias l~ Shell\Get-DirectoryHome
function Get-DirectoryHome {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME $Path
  }
  Microsoft.PowerShell.Management\Get-ChildItem @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias lc Shell\Get-DirectoryCode
function Get-DirectoryCode {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME\code $Path
  }
  Microsoft.PowerShell.Management\Get-ChildItem @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias l/ Shell\Get-DirectoryDrive
function Get-DirectoryDrive {
  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location).Drive.Root $Path
  }
  Microsoft.PowerShell.Management\Get-ChildItem @FullPath @args
}
