using namespace System.Collections.Generic
using namespace Completer.PathCompleter

function Invoke-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path
  )
  if (-not $env:SSH_CLIENT) {
    if (-not $Path) {
      Invoke-Item -Path $PWD.Path @args
    }
    elseif (Test-Path -Path $Path -PathType Container) {
      Invoke-Item -Path $Path @args
    }
    else {
      throw (Test-Path -Path $Path -PathType Leaf) ? (
        [System.IO.IOException]::new(
          "The path '$Path' is a file, not a directory."
        )
      ) : (
        [System.IO.DirectoryNotFoundException]::new(
          "The directory path '$Path' does not exist."
        )
      )
    }
  }
  else {
    Write-Warning -Message 'Cannot open File Explorer over SSH session'
  }
}

function Invoke-DirectorySibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      $null, $null
    )]
    [string]$Path
  )

  Invoke-Directory -Path (
    Join-Path (Split-Path $PWD.Path) $Path
  ) @args
}

function Invoke-DirectoryRelative {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      $null, $null
    )]
    [string]$Path
  )

  Invoke-Directory -Path (
    Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  ) @args
}

function Invoke-DirectoryHome {

  [OutputType([void])]
  param(

    [LocationPathCompletions(
      '~',
      $null, $null
    )]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $HOME $Path) @args
}

function Invoke-DirectoryCode {

  [OutputType([void])]
  param(

    [LocationPathCompletions(
      '~\code',
      $null, $null
    )]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $REPO_ROOT $Path) @args
}

function Invoke-DirectoryDrive {

  [OutputType([void])]
  param(

    [LocationPathCompletions(
      '\',
      $null, $null
    )]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $PWD.Drive.Root $Path) @args
}

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

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList $Private:ArgumentList
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

  Invoke-Workspace @PSBoundParameters -Location (Split-Path $PWD.Path) -Empty
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

  Invoke-Workspace @PSBoundParameters -Location ($PWD.Path | Split-Path | Split-Path)
}

function Invoke-WorkspaceHome {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [LocationPathCompletions(
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

  Invoke-Workspace @PSBoundParameters -Location $HOME
}

function Invoke-WorkspaceCode {
  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [LocationPathCompletions(
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

  Invoke-Workspace @PSBoundParameters -Location $REPO_ROOT
}

New-Alias e Invoke-Directory
New-Alias e. Invoke-DirectorySibling
New-Alias e.. Invoke-DirectoryRelative
New-Alias eh Invoke-DirectoryHome
New-Alias ec Invoke-DirectoryCode
New-Alias e/ Invoke-DirectoryDrive

New-Alias i Invoke-Workspace
New-Alias i. Invoke-WorkspaceSibling
New-Alias i.. Invoke-WorkspaceRelative
New-Alias ih Invoke-WorkspaceHome
New-Alias ic Invoke-WorkspaceCode
