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
New-Alias .. Shell\Set-DirectorySibling
function Set-DirectorySibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Set-Location @FullPath @args
}

New-Alias c.. Shell\Set-DirectoryRelative
New-Alias ... Shell\Set-DirectoryRelative
function Set-DirectoryRelative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
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
    Path = Join-Path '~' $Path
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
    Path = Join-Path '~\code' $Path
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
    Path = Join-Path '\' $Path
  }

  Set-Location @FullPath @args
}

New-Alias d/ Shell\Set-DriveD
function Set-DriveD {
  param (
    [PathCompletions('D:\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path 'D:\' $Path
  }

  Set-Location @FullPath @args
}
