New-Alias explore Invoke-Directory
New-Alias e Invoke-Directory
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Directory {
  param(
    [PathCompletions(".", "")]
    [string]$Path
  )

  if ($env:SSH_CLIENT) {
    Read-Item -Path $Path
  }
  else {
    if ($Path) {
      if (Test-Path -Path $Path -PathType Leaf) {
        Edit-Item -Path $Path @args
      }
      else {
        Invoke-Item -Path $Path @args
      }
    }
    else {
      Invoke-Item -Path $PWD.Path @args
    }
  }
}

New-Alias e. Invoke-Sibling
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

New-Alias e.. Invoke-Relative
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path (Split-Path (Split-Path $PWD.Path)) $Path) @args
}

New-Alias e~ Invoke-Home
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $HOME $Path) @args
}

New-Alias ec Invoke-Code
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Code {
  param (
    [PathCompletions("~\code", "")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path ~\code $Path) @args
}

New-Alias e\ Invoke-Drive
New-Alias e/ Invoke-Drive
<#
.FORWARDHELPTARGETNAME Invoke-Item
.FORWARDHELPCATEGORY Function
#>
function Invoke-Drive {
  param (
    [PathCompletions("\", "")]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $PWD.Drive.Root $Path) @args
}
