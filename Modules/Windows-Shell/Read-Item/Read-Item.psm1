New-Alias p Read-Item
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Item {
  [OutputType(
    [string[]],
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]])
  ]
  param(
    [Parameter(Position = 0)]
    [PathCompletions(".", "")]
    [string]$Path,
    [Parameter()]
    [string]$RootPath
  )

  $Argument = ''
  $Container = @{
    PathType = 'Container'
  }

  if ($RootPath) {
    if (-not (Test-Path -Path $RootPath @Container)) {
      $Argument = $RootPath
      $RootPath = ''
    }
  }

  $FullPath = $RootPath ? (Join-Path $RootPath $Path) : $Path

  $Item = @{
    Path = $FullPath
  }

  if ($Path) {
    if (Test-Path @Item) {
      if (Test-Path @Item @Container) {
        if ($Argument) {
          Get-ChildItem @Item $Argument @args
        }
        else {
          Get-ChildItem @Item @args
        }
      }
      else {
        if ($Argument) {
          Get-Content @Item $Argument @args
        }
        else {
          Get-Content @Item @args
        }
      }
    }
    else {
      throw "Path '$FullPath' does not exist."
    }
  }
  else {
    if ($FullPath) {
      if ($Argument) {
        Get-ChildItem @Item $Argument @args
      }
      else {
        Get-ChildItem @Item @args
      }
    }
    else {
      Get-ChildItem @args
    }
  }
}

New-Alias p. Read-Sibling
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath ".." @args
}

New-Alias p.. Read-Relative
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath "..\.." @args
}

New-Alias p~ Read-Home
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath "~" @args
}

New-Alias pc Read-Code
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Code {
  [OutputType([void])]
  param (
    [PathCompletions("$CODE", "")]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath $CODE @args
}

New-Alias p\ Read-Drive
New-Alias p/ Read-Drive
<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Function
#>
function Read-Drive {
  param (
    [PathCompletions("\", "")]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath "\" @args
}
