New-Alias explore Invoke-Folder
New-Alias e Invoke-Folder
function Invoke-Folder {
  param([System.String]$Path = $PWD.Path)

  if ($env:SSH_CLIENT) {
    throw 'Cannot launch File Explorer from SSH client.'
  }

  if (Test-Path $Path -PathType Container) {
    Invoke-Item $Path @args
  }
  else {
    Edit-File $Path @args
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [ValidateSet([SiblingItem])]
    [System.String]$Path
  )
  Invoke-Folder (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [ValidateSet([RelativeItem])]
    [System.String]$Path
  )
  Invoke-Folder (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [ValidateSet([HomeItem])]
    [System.String]$Path
  )
  Invoke-Folder (Join-Path $HOME $Path) @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [ValidateSet([DriveItem])]
    [System.String]$Path
  )
  Invoke-Folder (Join-Path $PWD.Drive.Root $Path) @args
}
