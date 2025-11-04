New-Alias explore Invoke-Folder
New-Alias e Invoke-Folder
function Invoke-Folder {
  param([string]$Path)

  if ($env:SSH_CLIENT) {
    throw 'Cannot launch File Explorer from SSH client.'
  }

  if ($Path) {
    if (Test-Path $Path -PathType Leaf) {
      Edit-File $Path @args
    }
    else {
      Invoke-Item $Path @args
    }
  }
  else {
    Invoke-Item $PWD.Path @args
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [ValidateSet([SiblingItem])]
    [string]$Path
  )
  Invoke-Folder (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [ValidateSet([RelativeItem])]
    [string]$Path
  )
  Invoke-Folder (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [ValidateSet([HomeItem])]
    [string]$Path
  )
  Invoke-Folder (Join-Path $HOME $Path) @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [ValidateSet([DriveItem])]
    [string]$Path
  )
  Invoke-Folder (Join-Path $PWD.Drive.Root $Path) @args
}
