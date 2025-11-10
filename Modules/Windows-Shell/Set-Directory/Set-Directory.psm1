New-Alias c Set-Directory
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Directory {
  [OutputType([void])]
  param (
    [PathCompletions(".", "Directory")]
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
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Sibling {
  [OutputType([void])]
  param (
    [PathCompletions("..", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path $PWD.Path) $Path)
}

New-Alias c.. Set-Relative
New-Alias ... Set-Relative
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Relative {
  [OutputType([void])]
  param (
    [PathCompletions("..\..", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path)
}

New-Alias c~ Set-Home
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Home {
  [OutputType([void])]
  param (
    [PathCompletions("~", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path $HOME $Path)
}

New-Alias cc Set-Code
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Code {
  [OutputType([void])]
  param (
    [PathCompletions("~\code", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path "~\code" $Path)
}

New-Alias c\ Set-Drive
New-Alias c/ Set-Drive
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-Drive {
  [OutputType([void])]
  param (
    [PathCompletions("\", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path "\" $Path)
}

New-Alias d\ Set-DriveD
New-Alias d/ Set-DriveD
<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Function
#>
function Set-DriveD {
  [OutputType([void])]
  param (
    [PathCompletions("D:\", "Directory")]
    [string]$Path
  )

  Set-Location -Path (Join-Path 'D:\' $Path)
}
