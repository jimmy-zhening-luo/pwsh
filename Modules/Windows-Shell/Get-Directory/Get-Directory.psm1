New-Alias l Get-Directory
function Get-Directory {
  param (
    [PathCompletions('.', 'Directory')]
    [string]$Path
  )

  if ($Path) {
    Get-ChildItem -Path $Path @args
  }
  else {
    Get-ChildItem @args
  }
}

New-Alias l. Get-Sibling
function Get-Sibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias l~ Get-Home
function Get-Home {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path '~' $Path) @args
}

New-Alias lc Get-Code
function Get-Code {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path '~\code' $Path) @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path '\' $Path) @args
}
