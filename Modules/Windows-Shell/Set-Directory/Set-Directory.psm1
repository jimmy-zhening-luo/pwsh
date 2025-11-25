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

New-Alias c. Set-Sibling
New-Alias .. Set-Sibling
function Set-Sibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Set-Location @FullPath @args
}

New-Alias c.. Set-Relative
New-Alias ... Set-Relative
function Set-Relative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
  }

  Set-Location @FullPath @args
}

New-Alias c~ Set-Home
function Set-Home {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~' $Path
  }

  Set-Location @FullPath @args
}

New-Alias cc Set-Code
function Set-Code {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~\code' $Path
  }

  Set-Location @FullPath @args
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
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

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
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
