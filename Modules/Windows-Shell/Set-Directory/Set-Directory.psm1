New-Alias c Set-Location

New-Alias cc Set-Code
function Set-Code {
  [OutputType([void])]
  param ()

  Set-Location $CODE
}

New-Alias c. Set-Sibling
function Set-Sibling {
  [OutputType([void])]
  param (
    [ValidateSet([SiblingDirectory])]
    [string]$Path
  )

  Set-Location (Join-Path (Split-Path $PWD.Path) $Path)
}

New-Alias c.. Set-Relative
function Set-Relative {
  [OutputType([void])]
  param (
    [ValidateSet([RelativeDirectory])]
    [string]$Path
  )

  Set-Location (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path)
}

New-Alias c~ Set-Home
function Set-Home {
  [OutputType([void])]
  param (
    [ValidateSet([HomeDirectory])]
    [string]$Path
  )

  Set-Location (Join-Path $HOME $Path)
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
function Set-Drive {
  [OutputType([void])]
  param (
    [ValidateSet([DriveDirectory])]
    [string]$Path
  )

  Set-Location (Join-Path $PWD.Drive.Root $Path)
}

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
function Set-DriveD {
  [OutputType([void])]
  param (
    [ValidateSet([DriveDDirectory])]
    [string]$Path
  )

  Set-Location (Join-Path 'D:\' $Path)
}
