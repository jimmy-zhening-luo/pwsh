using namespace System.Collections.Generic

function Invoke-Workspace {

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

  $Private:ArgumentList = [List[string]]::new()
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
      [string]$Private:FullPath = Resolve-Path $Target

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
    if (-not $ProfileName.StartsWith('-')) {
      $Window = $True

      $ArgumentList.Add('--profile')
    }

    $ArgumentList.Add($ProfileName)
  }

  if ($Window) {
    $ArgumentList.Add('--new-window')
  }
  elseif ($ReuseWindow) {
    $ArgumentList.Add('--reuse-window')
  }

  [hashtable]$Private:Process = @{
    FilePath     = 'code.cmd'
    ArgumentList = $ArgumentList
    NoNewWindow  = $True
  }
  Start-Process @Process
}

function Invoke-WorkspaceSibling {

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
    Location = $PWD | Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceRelative {

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
    Location = $PWD | Split-Path | Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

function Invoke-WorkspaceHome {

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

  [OutputType([void])]

  param(

    [PathCompletions('\')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
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
