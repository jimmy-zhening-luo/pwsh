New-Alias c Set-Location

New-Alias cc Set-Code
function Set-Code {
  Set-Location $CODE
}

New-Alias c. Set-Sibling
function Set-Sibling {
  param (
    [ValidateSet([SiblingFolder])]
    [System.String]$Path
  )
  Set-Location (Join-Path (Split-Path -Parent $PWD) $Path)
}

New-Alias c.. Set-Relative
function Set-Relative {
  param (
    [ValidateSet([RelativeFolder])]
    [System.String]$Path
  )
  Set-Location (Join-Path (Split-Path -Parent (Split-Path -Parent $PWD)) $Path)
}

New-Alias c~ Set-Home
function Set-Home {
  param (
    [ValidateSet([HomeFolder])]
    [System.String]$Path
  )
  Set-Location (Join-Path $HOME $Path)
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
function Set-Drive {
  param (
    [ValidateSet([DriveFolder])]
    [System.String]$Path
  )
  Set-Location (Join-Path $PWD.Drive.Root $Path)
}

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
function Set-DriveD {
  param (
    [ValidateSet([DriveDFolder])]
    [System.String]$Path
  )
  Set-Location (Join-Path 'D:\' $Path)
}
