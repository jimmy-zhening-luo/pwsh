using namespace System.Collections.Generic
using namespace Module.Completer
using namespace Module.Completer.PathCompleter

function Start-Workspace {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('i')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions()]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [DynamicCompletions(
      {
        return (
          Get-Content -Raw $Env:APPDATA\Code\User\sync\profiles\lastSyncprofiles.json |
            ConvertFrom-Json |
            Select-Object -ExpandProperty syncData |
            Select-Object -ExpandProperty content |
            ConvertFrom-Json
        ).Name.ToLower() + 'default'
      }
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [Parameter()]
    [AllowEmptyString()]
    [string]$Location,

    [switch]$Window,

    [switch]$ReuseWindow,

    [switch]$Empty
  )

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  $ArgumentList = [List[string]]::new()

  if (
    $Location -and !(
      Test-Path $Location -PathType Container
    )
  ) {
    $ArgumentList.Add($Location)
    $Location = ''
  }

  if ($Workspace) {
    [string]$Target = $Location ? (
      Join-Path $Location $Workspace
    ) : $Workspace
    if (Test-Path $Target) {
      $ArgumentList.Insert(
        0,
        (Resolve-Path $Target).Path
      )
    }
    else {
      if (!$Workspace.StartsWith([char]'-')) {
        throw "Path '$Workspace' does not exist."
      }

      $ArgumentList.Insert(0, $Workspace)
      $Workspace = ''
    }
  }

  if (!$Workspace) {
    if ($Location -and !$Empty -or $ReuseWindow) {
      $ArgumentList.Insert(
        0,
        (
          $Location ? (
            Resolve-Path $Location
          ) : $PWD
        ).Path
      )
    }
  }

  if ($Argument) {
    $ArgumentList.AddRange(
      [List[string]]$Argument
    )
  }
  if ($ProfileName) {
    $ArgumentList.Add(
      $ProfileName.StartsWith([char]'-') ? $ProfileName : "--profile=$ProfileName"
    )
  }

  [string[]]$FullProfileArgumentMatch = $ArgumentList -match '^(?>--profile=(?>[^\s=-][^=]*)?)$'
  [string[]]$ProfileArgumentMatch = $ArgumentList -eq '--profile'

  if ($FullProfileArgumentMatch -or $ProfileArgumentMatch) {
    if (
      $FullProfileArgumentMatch -and $ProfileArgumentMatch -or $FullProfileArgumentMatch.Count -gt 1 -or $ProfileArgumentMatch.Count -gt 1
    ) {
      throw 'Visual Studio Code profile argument was specified more than once.'
    }

    [string]$ProfileArgumentValue = ''

    if ($FullProfileArgumentMatch) {
      [string]$_profile, [string]$ProfileArgumentValue = $FullProfileArgumentMatch[0] -split '='

      [void]$ArgumentList.Remove($FullProfileArgumentMatch[0])
    }
    else {
      $ProfileArgumentIndex = $ArgumentList.IndexOf($ProfileArgumentMatch[0])

      if (
        $ProfileArgumentIndex -eq -1 -or $ProfileArgumentIndex -eq (
          $ArgumentList.Count - 1
        )
      ) {
        throw 'Visual Studio Code --profile argument was specified but no profile name was given.'
      }

      [string]$ProfileArgumentValue = $ArgumentList[$ProfileArgumentIndex + 1]

      if (
        [string]::IsNullOrWhiteSpace(
          $ProfileArgumentValue
        ) -or $ProfileArgumentValue.StartsWith(
          [char]'-'
        ) -or $ProfileArgumentValue.Contains(
          [char]'='
        )
      ) {
        throw 'Visual Studio Code --profile argument is missing or invalid.'
      }

      $ArgumentList.RemoveRange($ProfileArgumentIndex, 2)
    }

    $Profiles = Get-Content -Raw $Env:APPDATA\Code\User\sync\profiles\lastSyncprofiles.json |
      ConvertFrom-Json |
      Select-Object -ExpandProperty syncData |
      Select-Object -ExpandProperty content |
      ConvertFrom-Json |
      Select-Object -ExpandProperty Name

    $TrimmedProfileName = $ProfileArgumentValue.Trim()

    [string]$MatchedProfile = $Profiles |
      Where-Object {
        $PSItem.StartsWith(
          $TrimmedProfileName,
          [System.StringComparison]::OrdinalIgnoreCase
        )
      } |
      Select-Object -First 1

    if (!$MatchedProfile) {
      throw "Visual Studio Code profile '$TrimmedProfileName' does not exist."
    }

    $ArgumentList.Add("--profile=$MatchedProfile")
    $Window = $True
  }

  if ($Window) {
    if ($ArgumentList.Contains('--reuse-window')) {
      [void]$ArgumentList.Remove('--reuse-window')
    }

    if (!$ArgumentList.Contains('--new-window')) {
      $ArgumentList.Add('--new-window')
    }
  }
  elseif ($ReuseWindow) {
    if ($ArgumentList.Contains('--new-window')) {
      throw 'Cannot specify --reuse-window when --new-window is specified, as --new-window will always take precedence.'
    }

    if (!$ArgumentList.Contains('--reuse-window')) {
      $ArgumentList.Add('--reuse-window')
    }
  }

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList $ArgumentList
}

function Start-WorkspaceSibling {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('i.', 'ix')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions(
      '..'
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Start-Workspace @PSBoundParameters -Location (Split-Path $PWD.Path) -Empty
}

function Start-WorkspaceRelative {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('i..', 'ixx')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions(
      '..\..'
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Start-Workspace @PSBoundParameters -Location ($PWD.Path | Split-Path | Split-Path)
}

function Start-WorkspaceHome {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('ih')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions(
      '~'
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Start-Workspace @PSBoundParameters -Location $HOME
}

function Start-WorkspaceCode {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('ic')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions(
      '~\code'
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Start-Workspace @PSBoundParameters -Location $REPO_ROOT
}

function Start-WorkspaceDrive {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('i/')]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions(
      '\'
    )]
    [string]$Workspace,

    [Parameter(
      Position = 1
    )]
    [string]$ProfileName,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Start-Workspace @PSBoundParameters -Location $PWD.Drive.Root
}
