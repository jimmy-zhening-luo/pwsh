New-Alias l Get-Directory
function Get-Directory {
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

New-Alias l. Get-Sibling
function Get-Sibling {
  param (
    [PathCompletions('..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l.. Get-Relative
function Get-Relative {
  param (
    [PathCompletions('..\..', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path (Split-Path (Split-Path $PWD.Path)) $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l~ Get-Home
function Get-Home {
  param (
    [PathCompletions('~', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~' $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias lc Get-Code
function Get-Code {
  param (
    [PathCompletions('~\code', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '~\code' $Path
  }

  Get-ChildItem @FullPath @args
}

New-Alias l\ Get-Drive
New-Alias l/ Get-Drive
function Get-Drive {
  param (
    [PathCompletions('\', 'Directory')]
    [string]$Path
  )

  $FullPath = @{
    Path = Join-Path '\' $Path
  }

  Get-ChildItem @FullPath @args
}
