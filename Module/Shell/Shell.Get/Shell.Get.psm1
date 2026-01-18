using namespace System.Collections.Generic
using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-File {

  [OutputType([string[]])]
  [Alias('p')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      '',
      [PathItemType]::File
    )]
    [string]$Path,

    [string]$Location
  )

  $Argument = [List[string]]::new()

  if (
    $Location -and -not (
      Test-Path $Location -PathType Container
    )
  ) {
    $Argument.Add($Location)
    $Location = ''
  }

  [string]$FullPath = $Location ? (
    Join-Path $Location $Path
  ) : $Path

  if (-not (Test-Path $FullPath)) {
    throw "Path '$Target' does not exist."
  }
  elseif (-not (Test-Path $FullPath -PathType Leaf)) {
    throw "Path '$Target' is not a leaf item."
  }

  if ($args) {
    foreach ($i in $args) {
      $Argument.Add($i)
    }
  }

  return Get-Content -Path $FullPath @Argument
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileSibling {

  [OutputType([string])]
  [Alias('p.')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      '..',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location (Split-Path $PWD.Path) @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileRelative {

  [OutputType([string])]
  [Alias('p..')]
  param(

    [RelativePathCompletions(
      '..\..',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location ($PWD.Path | Split-Path | Split-Path) @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileHome {

  [OutputType([string])]
  [Alias('ph')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions(
      '~',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $HOME @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileCode {

  [OutputType([string])]
  [Alias('pc')]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $REPO_ROOT @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileDrive {

  [OutputType([string])]
  [Alias('p/')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      '\',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $PWD.Drive.Root @args
}
