using namespace PathCompleter

function Set-Directory {

  [OutputType([void])]

  param(

    [PathCompletions('.', [PathItemType]::Directory)]
    [string]$Path

  )

  if ($Path -or $args) {
    Set-Location @PSBoundParameters @args
  }
  else {
    Set-Location -Path (Split-Path $PWD.Path)
  }
}

function Set-DirectorySibling {

  [OutputType([void])]

  param(

    [PathCompletions('..', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path (Split-Path $PWD.Path) $Path
  }
  Set-Location @FullPath @args
}

function Set-DirectoryRelative {

  [OutputType([void])]

  param(

    [PathCompletions('..\..', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  }
  Set-Location @FullPath @args
}

function Set-DirectoryHome {

  [OutputType([void])]

  param(

    [PathCompletions('~', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME $Path
  }
  Set-Location @FullPath @args
}

function Set-DirectoryCode {

  [OutputType([void])]

  param(

    [PathCompletions('~\code', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $HOME code $Path
  }
  Set-Location @FullPath @args
}

function Set-Drive {

  [OutputType([void])]

  param(

    [PathCompletions('\', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path $PWD.Drive.Root $Path
  }
  Set-Location @FullPath @args
}

function Set-DriveD {

  [OutputType([void])]

  param(

    [PathCompletions('D:', [PathItemType]::Directory)]
    [string]$Path

  )

  [hashtable]$Private:FullPath = @{
    Path = Join-Path D: $Path
  }
  Set-Location @FullPath @args
}

New-Alias c Set-Directory
New-Alias c. Set-DirectorySibling
New-Alias c.. Set-DirectoryRelative
New-Alias ch Set-DirectoryHome
New-Alias cc Set-DirectoryCode
New-Alias c/ Set-Drive
New-Alias d/ Set-DriveD
