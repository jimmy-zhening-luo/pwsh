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
      '.',
      $null, $null, $null
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

    [switch]$Empty

  )

  $Private:ArgumentList = [List[string]]::new()
  if ($CodeArgument) {
    $ArgumentList.AddRange(
      [List[string]]$CodeArgument
    )
  }

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $ArgumentList.Insert(0, $Location)
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
    if (-not $ReuseWindow -and ($Empty -or -not $Location)) {
      #
    }
    else {
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

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
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
    FilePath     = "$HOME\AppData\Local\Programs\Microsoft VS Code\bin\code.cmd"
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
    [PathCompletions(
      '..',
      $null, $null, $null
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
    [switch]$ReuseWindow

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
    [PathCompletions(
      '..\..',
      $null, $null, $null
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
    [switch]$ReuseWindow

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
    [PathCompletions(
      '~',
      $null, $null, $null
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
    [switch]$ReuseWindow

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
    [PathCompletions(
      '~\code',
      $null, $null, $null
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
    [switch]$ReuseWindow

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
