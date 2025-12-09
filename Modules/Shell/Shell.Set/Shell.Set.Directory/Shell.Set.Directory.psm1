New-Alias c Shell\Set-Directory
function Set-Directory {
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Set-Location @PSBoundParameters @args
  }
  else {
    Set-Location @args
  }
}

New-Alias c. Shell\Set-DirectorySibling
function Set-DirectorySibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location | Split-Path) $Path
  }
  Set-Location @FullPath @args
}

New-Alias c.. Shell\Set-DirectoryRelative
function Set-DirectoryRelative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location | Split-Path | Split-Path) $Path
  }
  Set-Location @FullPath @args
}

New-Alias c~ Shell\Set-DirectoryHome
function Set-DirectoryHome {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Set-Location @FullPath @args
}

New-Alias cc Shell\Set-DirectoryCode
function Set-DirectoryCode {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $HOME\code $Path
  }
  Set-Location @FullPath @args
}

New-Alias c/ Shell\Set-Drive
function Set-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Get-Location).Drive.Root $Path
  }
  Set-Location @FullPath @args
}

New-Alias d/ Shell\Set-DriveD
function Set-DriveD {
  param (
    [PathCompletions('D:', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path D: $Path
  }
  Set-Location @FullPath @args
}
