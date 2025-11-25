New-Alias c Set-Directory
function Set-Directory {
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Set-Location -Path $Path @args
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

  Set-Location -Path (Join-Path (Split-Path $PWD.Path) $Path)
}

New-Alias c.. Set-Relative
New-Alias ... Set-Relative
function Set-Relative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path)
}

New-Alias c~ Set-Home
function Set-Home {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  Set-Location -Path (Join-Path '~' $Path)
}

New-Alias cc Set-Code
function Set-Code {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  Set-Location -Path (Join-Path '~\code' $Path)
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
function Set-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  Set-Location -Path (Join-Path '\' $Path)
}

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
function Set-DriveD {
  param (
    [PathCompletions('D:\', 'Directory')]
    [string]$Path
  )

  Set-Location -Path (Join-Path 'D:\' $Path)
}
