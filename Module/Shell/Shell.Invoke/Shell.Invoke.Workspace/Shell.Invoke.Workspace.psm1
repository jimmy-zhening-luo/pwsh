using namespace System.Collections.Generic

function Invoke-Workspace {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [PathCompletions('.')]
    [string]$Path,

    [Alias('Name')]
    [string]$ProfileName,

    [switch]$Window,

    [switch]$ReuseWindow,

    [string]$Location

  )

  [List[string]]$Private:ArgumentList = [List[string]]::new()
  if ($args) {
    $ArgumentList.AddRange(
      [List[string]]$args
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
    [string]$Private:Target = $Location ? (Join-Path $Location $Path) : $Path

    if (Test-Path -Path $Target) {
      [string]$Private:FullPath = (Resolve-Path $Target).Path

      $ArgumentList.Insert(0, $FullPath)
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Target' does not exist."
      }

      if (-not $Location) {
        $Location = $PWD.Path
      }

      $ArgumentList.Insert(0, $Location)
    }
  }
  else {
    if (-not $Location) {
      $Location = $PWD.Path
    }

    $ArgumentList.Insert(0, $Location)
  }

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  if ($ProfileName) {
    $Window = $True
    $ArgumentList.Add(
      $ProfileName.StartsWith('-') ? (
        $ProfileName
      ) : (
        "--profile=$ProfileName"
      )
    )
  }

  if ($Window) {
    $ArgumentList.Add('--new-window')
  }
  elseif ($ReuseWindow) {
    $ArgumentList.Add('--reuse-window')
  }

  [hashtable]$Private:Process = @{
    FilePath     = "$HOME\AppData\Local\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = $ArgumentList
    NoNewWindow  = $True
  }
  Start-Process @Process
}

function Invoke-WorkspaceSibling {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [PathCompletions('..')]
    [string]$Path,

    [Alias('Name', 'pn')]
    [string]$ProfileName,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow

  )

  [hashtable]$Private:Location = @{
    Location = Split-Path $PWD.Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceRelative {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [PathCompletions('..\..')]
    [string]$Path,

    [Alias('Name', 'pn')]
    [string]$ProfileName,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Path | Split-Path | Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceHome {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [PathCompletions('~')]
    [string]$Path,

    [Alias('Name', 'pn')]
    [string]$ProfileName,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow

  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceCode {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [PathCompletions('~\code')]
    [string]$Path,

    [Alias('Name', 'pn')]
    [string]$ProfileName,

    [switch]$Window,

    [Alias('rw')]
    [switch]$ReuseWindow

  )

  [hashtable]$Private:Location = @{
    Location = "$HOME\code"
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceDrive {

  [CmdletBinding()]

  [OutputType([void])]

  param(

    [Parameter(
      Position = 0
    )]
    [AllowEmptyString()]
    [PathCompletions('\')]
    [string]$Path,

    [Parameter(
      Position = 1
    )]
    [AllowEmptyString()]
    [Alias('Name', 'pn')]
    [string]$ProfileName,

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
    Location = $PWD.Drive.Root
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

New-Alias i Invoke-Workspace
New-Alias i. Invoke-WorkspaceSibling
New-Alias i.. Invoke-WorkspaceRelative
New-Alias ih Invoke-WorkspaceHome
New-Alias ic Invoke-WorkspaceCode
New-Alias i/ Invoke-WorkspaceDrive
