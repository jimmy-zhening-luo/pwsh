using namespace System.Collections.Generic
using namespace Completer.PathCompleter

function Invoke-Directory {

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

function Invoke-DirectorySibling {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return Split-Path $PWD.Path }
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
      { return $PWD.Path | Split-Path | Split-Path }
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

    [PathCompletions(
      '~'
    )]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $HOME $Path) @args
}

function Invoke-DirectoryCode {

  [OutputType([void])]
  param(

    [PathCompletions(
      '~\code'
    )]
    [string]$Path
  )

  Invoke-Directory -Path (Join-Path $REPO_ROOT $Path) @args
}

function Invoke-DirectoryDrive {

  [OutputType([void])]
  param(

    [PathCompletions(
      '\'
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
      { return $PWD.Path }
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
      if (-not $Workspace.StartsWith('-')) {
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
    $Window = $True
    $ArgumentList.Add(
      $ProfileName.StartsWith('-') ? $ProfileName : "--profile=$ProfileName"
    )
  }

  if ($Window) {
    $ArgumentList.Add('--new-window')
  }
  elseif ($ReuseWindow) {
    $ArgumentList.Add('--reuse-window')
  }

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList $ArgumentList
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

  Invoke-Workspace @PSBoundParameters -Location $REPO_ROOT
}

New-Alias i/ Invoke-WorkspaceDrive
function Invoke-WorkspaceDrive {
  [CmdletBinding()]
  [OutputType([void])]
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

  Invoke-Workspace @PSBoundParameters -Location $PWD.Drive.Root
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
