New-Alias explore Invoke-Folder
New-Alias e Invoke-Folder
function Invoke-Folder {
  param([System.String]$Path = $PWD)

  if (Test-Path -Path $Path -PathType Leaf) {
    Edit-File -Path $Path @args
  }
  else {
    Invoke-Item -Path $Path @args
  }
}

New-Alias e. Invoke-Sibling
function Invoke-Sibling {
  param (
    [ValidateSet([SiblingItem])]
    [System.String]$Path
  )
  Invoke-Folder -Path (Join-Path (Split-Path -Parent $PWD) $Path) @args
}

New-Alias e.. Invoke-Relative
function Invoke-Relative {
  param (
    [ValidateSet([RelativeItem])]
    [System.String]$Path
  )
  Invoke-Folder -Path (Join-Path (Split-Path -Parent (Split-Path -Parent $PWD)) $Path) @args
}

New-Alias e~ Invoke-Home
function Invoke-Home {
  param (
    [ValidateSet([HomeItem])]
    [System.String]$Path
  )
  Invoke-Folder -Path (Join-Path $HOME $Path) @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
function Invoke-Drive {
  param (
    [ValidateSet([DriveItem])]
    [System.String]$Path
  )
  Invoke-Folder -Path (Join-Path $PWD.Drive.Root $Path) @args
}
