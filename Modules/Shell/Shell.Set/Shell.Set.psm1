New-Alias c Set-Directory
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

New-Alias c. Set-DirectorySibling
function Set-DirectorySibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path ($PWD | Split-Path) $Path
  }
  Set-Location @FullPath @args
}

New-Alias c.. Set-DirectoryRelative
function Set-DirectoryRelative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path ($PWD | Split-Path | Split-Path) $Path
  }
  Set-Location @FullPath @args
}

New-Alias ch Set-DirectoryHome
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

New-Alias cc Set-DirectoryCode
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

New-Alias c/ Set-Drive
function Set-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Set-Location @FullPath @args
}

New-Alias d/ Set-DriveD
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
