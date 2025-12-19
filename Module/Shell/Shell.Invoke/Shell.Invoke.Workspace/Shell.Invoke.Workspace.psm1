using namespace System.Collections.Generic
using namespace Completer.PathCompleter

function Invoke-Workspace {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$Argument,

    [Parameter()]
    [AllowEmptyString()]
    [string]$Location,

    [switch]$Window,

    [switch]$ReuseWindow,

    [switch]$Empty,

    [Parameter(DontShow)][switch]$zNothing

  )

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  $Private:ArgumentList = [List[string]]::new()

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Private:ArgumentList.Add($Location)
    $Location = ''
  }

  if ($Workspace) {
    [string]$Private:Target = $Location ? (
      Join-Path $Location $Workspace
    ) : $Workspace
    if (Test-Path -Path $Private:Target) {
      $Private:ArgumentList.Insert(
        0,
        [string](Resolve-Path -Path $Target).Path
      )
    }
    else {
      if (-not $Workspace.StartsWith('-')) {
        throw "Path '$Workspace' does not exist."
      }

      $Private:ArgumentList.Insert(0, $Workspace)
      $Workspace = ''
    }
  }

  if (-not $Workspace) {
    if ($Location -and -not $Empty -or $ReuseWindow) {
      $Private:ArgumentList.Insert(
        0,
        [string](
          $Location ? (
            Resolve-Path -Path $Location
          ) : (
            $PWD
          )
        ).Path
      )
    }
  }

  if ($Argument) {
    $Private:ArgumentList.AddRange(
      [List[string]]$Argument
    )
  }

  if ($ProfileName) {
    $Window = $True
    $Private:ArgumentList.Add(
      $ProfileName.StartsWith('-') ? (
        $ProfileName
      ) : (
        "--profile=$ProfileName"
      )
    )
  }

  if ($Window) {
    $Private:ArgumentList.Add('--new-window')
  }
  elseif ($ReuseWindow) {
    $Private:ArgumentList.Add('--reuse-window')
  }

  [hashtable]$Private:VSCode = @{
    FilePath     = "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = $Private:ArgumentList
    NoNewWindow  = $True
  }
  Start-Process @Private:VSCode
}

function Invoke-WorkspaceSibling {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      $null, $null
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = Split-Path $PWD.Path
    Empty    = $True
  }
  Invoke-Workspace @PSBoundParameters @Private:Location
}

function Invoke-WorkspaceRelative {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      $null, $null
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Path | Split-Path | Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Private:Location
}

function Invoke-WorkspaceHome {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathLocationCompletions(
      '~',
      $null, $null
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Invoke-Workspace @PSBoundParameters @Private:Location
}

function Invoke-WorkspaceCode {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathLocationCompletions(
      '~\code',
      $null, $null
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = $REPO_ROOT
  }
  Invoke-Workspace @PSBoundParameters @Private:Location
}

New-Alias i Invoke-Workspace
New-Alias i. Invoke-WorkspaceSibling
New-Alias i.. Invoke-WorkspaceRelative
New-Alias ih Invoke-WorkspaceHome
New-Alias ic Invoke-WorkspaceCode
