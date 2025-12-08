Microsoft.PowerShell.Utility\New-Alias c Shell\Set-Directory
function Set-Directory {
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Microsoft.PowerShell.Management\Set-Location @PSBoundParameters @args
  }
  else {
    Microsoft.PowerShell.Management\Set-Location @args
  }
}

Microsoft.PowerShell.Utility\New-Alias c. Shell\Set-DirectorySibling
function Set-DirectorySibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias c.. Shell\Set-DirectoryRelative
function Set-DirectoryRelative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias c~ Shell\Set-DirectoryHome
function Set-DirectoryHome {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias cc Shell\Set-DirectoryCode
function Set-DirectoryCode {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME\code $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias c/ Shell\Set-Drive
function Set-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location).Drive.Root $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias d/ Shell\Set-DriveD
function Set-DriveD {
  param (
    [PathCompletions('D:', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path D: $Path
  }
  Microsoft.PowerShell.Management\Set-Location @FullPath @args
}
