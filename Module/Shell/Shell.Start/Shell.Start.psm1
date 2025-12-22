using namespace System.Collections.Generic
using namespace Completer
using namespace Completer.PathCompleter

function Start-Explorer {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    [string]$Path
  )
  if (-not $env:SSH_CLIENT) {
    if (-not $Path) {
      Invoke-Item -Path $PWD.Path @args
    }
    elseif (Test-Path $Path -PathType Container) {
      Invoke-Item -Path $Path @args
    }
    else {
      throw (
        Test-Path $Path -PathType Leaf
      ) ? [System.IO.IOException]::new(
        "The path '$Path' is a file, not a directory."
      ) : [System.IO.DirectoryNotFoundException]::new(
        "The directory path '$Path' does not exist."
      )
    }
  }
  else {
    Write-Warning -Message 'Cannot open File Explorer over SSH session'
  }
}

function Start-ExplorerSibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return Split-Path $PWD.Path }
    )]
    [string]$Path
  )

  Start-Explorer -Path (
    Join-Path (Split-Path $PWD.Path) $Path
  ) @args
}

function Start-ExplorerRelative {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path }
    )]
    [string]$Path
  )

  Start-Explorer -Path (
    Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  ) @args
}

function Start-ExplorerHome {

  [OutputType([void])]
  param(

    [PathCompletions(
      '~'
    )]
    [string]$Path
  )

  Start-Explorer -Path (Join-Path $HOME $Path) @args
}

function Start-ExplorerCode {

  [OutputType([void])]
  param(

    [PathCompletions(
      '~\code'
    )]
    [string]$Path
  )

  Start-Explorer -Path (Join-Path $REPO_ROOT $Path) @args
}

function Start-ExplorerDrive {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Drive.Root }
    )]
    [string]$Path
  )

  Start-Explorer -Path (Join-Path $PWD.Drive.Root $Path) @args
}

function Start-Workspace {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return $PWD.Path }
    )]
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

  $ArgumentList = [List[string]]::new()

  if (
    $Location -and -not (
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
      if (-not $Workspace.StartsWith([char]'-')) {
        throw "Path '$Workspace' does not exist."
      }

      $ArgumentList.Insert(0, $Workspace)
      $Workspace = ''
    }
  }

  if (-not $Workspace) {
    if ($Location -and -not $Empty -or $ReuseWindow) {
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
      $ProfileName.StartsWith('-') ? $ProfileName : "--profile=$ProfileName"
    )
  }

  [string[]]$FullProfileArgumentMatch = $ArgumentList -match [regex]'^(?>--profile=(?>[^\s=-][^=]*)?)$'
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

      $ArgumentList.Remove($FullProfileArgumentMatch[0]) | Out-Null
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

    [string[]]$Profiles = @('Default')
    $Profiles += Get-Content -Raw $Env:APPDATA\Code\User\sync\profiles\lastSyncprofiles.json |
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

    if (-not $MatchedProfile) {
      throw "Visual Studio Code profile '$TrimmedProfileName' does not exist."
    }

    $ArgumentList.Add("--profile=$MatchedProfile")
    $Window = $True
  }

  if ($Window) {
    if ($ArgumentList.Contains('--reuse-window')) {
      $ArgumentList.Remove('--reuse-window') | Out-Null
    }

    if (-not $ArgumentList.Contains('--new-window')) {
      $ArgumentList.Add('--new-window')
    }
  }
  elseif ($ReuseWindow) {
    if ($ArgumentList.Contains('--new-window')) {
      throw 'Cannot specify --reuse-window when --new-window is specified, as --new-window will always take precedence.'
    }

    if (-not $ArgumentList.Contains('--reuse-window')) {
      $ArgumentList.Add('--reuse-window')
    }
  }

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList $ArgumentList
}

function Start-WorkspaceSibling {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return Split-Path $PWD.Path }
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

  Start-Workspace @PSBoundParameters -Location (Split-Path $PWD.Path) -Empty
}

function Start-WorkspaceRelative {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path }
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

  Start-Workspace @PSBoundParameters -Location ($PWD.Path | Split-Path | Split-Path)
}

function Start-WorkspaceHome {
  [CmdletBinding()]
  [OutputType([void])]
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

  Start-Workspace @PSBoundParameters -Location $HOME
}

function Start-WorkspaceCode {
  [CmdletBinding()]
  [OutputType([void])]
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

  Start-Workspace @PSBoundParameters -Location $REPO_ROOT
}

New-Alias i/ Start-WorkspaceDrive
function Start-WorkspaceDrive {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [RelativePathCompletions(
      { return $PWD.Drive.Root }
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

  Start-Workspace @PSBoundParameters -Location $PWD.Drive.Root
}

New-Alias e Start-Explorer
New-Alias e. Start-ExplorerSibling
New-Alias e.. Start-ExplorerRelative
New-Alias eh Start-ExplorerHome
New-Alias ec Start-ExplorerCode
New-Alias e/ Start-ExplorerDrive

New-Alias i Start-Workspace
New-Alias i. Start-WorkspaceSibling
New-Alias i.. Start-WorkspaceRelative
New-Alias ih Start-WorkspaceHome
New-Alias ic Start-WorkspaceCode
