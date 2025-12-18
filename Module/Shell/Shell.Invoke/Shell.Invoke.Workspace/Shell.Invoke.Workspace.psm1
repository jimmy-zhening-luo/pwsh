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
    [PathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$Name,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$CodeArgument,

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
    $ArgumentList.Add($Location)
    $Location = ''
  }

  if ($Path) {
    [string]$Private:Target = $Location ? (
      Join-Path $Location $Path
    ) : $Path
    if (Test-Path -Path $Target) {
      $ArgumentList.Insert(
        0,
        [string](Resolve-Path -Path $Target).Path
      )
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Path' does not exist."
      }

      $ArgumentList.Insert(0, $Path)
      $Path = ''
    }
  }

  if (-not $Path) {
    if ($Location -and -not $Empty -or $ReuseWindow) {
      $ArgumentList.Insert(
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

  if ($CodeArgument) {
    $ArgumentList.AddRange(
      [List[string]]$CodeArgument
    )
  }

  if ($Name) {
    $Window = $True
    $ArgumentList.Add(
      $Name.StartsWith('-') ? (
        $Name
      ) : (
        "--profile=$Name"
      )
    )
  }

  if ($Window) {
    $ArgumentList.Add('--new-window')
  }
  elseif ($ReuseWindow) {
    $ArgumentList.Add('--reuse-window')
  }

  [hashtable]$Private:VSCode = @{
    FilePath     = "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = $ArgumentList
    NoNewWindow  = $True
  }
  Start-Process @VSCode
}

function Invoke-WorkspaceSibling {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathLocationCompletions(
      '..',
      $null, $null
    )]
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$Name,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$CodeArgument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = Split-Path $PWD.Path
    Empty    = $True
  }
  Invoke-Workspace @PSBoundParameters @Location
}

function Invoke-WorkspaceRelative {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathLocationCompletions(
      '..\..',
      $null, $null
    )]
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$Name,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$CodeArgument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Path | Split-Path | Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location
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
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$Name,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$CodeArgument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Invoke-Workspace @PSBoundParameters @Location
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
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [string]$Name,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments
    )]
    [string[]]$CodeArgument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow,

    [Parameter(DontShow)][switch]$zNothing

  )

  [hashtable]$Private:Location = @{
    Location = "$HOME\code"
  }
  Invoke-Workspace @PSBoundParameters @Location
}

New-Alias i Invoke-Workspace
New-Alias i. Invoke-WorkspaceSibling
New-Alias i.. Invoke-WorkspaceRelative
New-Alias ih Invoke-WorkspaceHome
New-Alias ic Invoke-WorkspaceCode
