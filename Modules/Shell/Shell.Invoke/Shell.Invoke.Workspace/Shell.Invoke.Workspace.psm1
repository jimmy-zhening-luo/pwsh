Microsoft.PowerShell.Utility\New-Alias i Shell\Invoke-Workspace
function Invoke-Workspace {
  [OutputType([void])]
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name')]
    [string]$ProfileName,
    [switch]$Window,
    [switch]$ReuseWindow,
    [string]$Location
  )

  $ArgumentList = $args

  if (
    $Location -and -not (
      Microsoft.PowerShell.Management\Test-Path -Path $Location -PathType Container
    )
  ) {
    $ArgumentList = , $Location + $ArgumentList
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Microsoft.PowerShell.Management\Join-Path $Location $Path) : $Path

    if (Microsoft.PowerShell.Management\Test-Path -Path $Target) {
      $FullPath = (Microsoft.PowerShell.Management\Resolve-Path $Target).Path
      $ArgumentList = , $FullPath + $ArgumentList
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Target' does not exist."
      }

      if (-not $Location) {
        $Location = $PWD.Path
      }

      $ArgumentList = $Location, $Path + $ArgumentList
    }
  }
  else {
    if (-not $Location) {
      $Location = $PWD.Path
    }

    $ArgumentList = , $Location + $ArgumentList
  }

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  if ($ProfileName) {
    if (-not $ProfileName.StartsWith('-')) {
      $Window = $True

      $ArgumentList += '--profile'
    }

    $ArgumentList += $ProfileName
  }

  if ($Window) {
    $ArgumentList += '--new-window'
  }
  elseif ($ReuseWindow) {
    $ArgumentList += '--reuse-window'
  }

  $Process = @{
    FilePath     = 'code.cmd'
    ArgumentList = $ArgumentList
    NoNewWindow  = $True
  }
  [void](Microsoft.PowerShell.Management\Start-Process @Process)
}

Microsoft.PowerShell.Utility\New-Alias i. Shell\Invoke-WorkspaceSibling
function Invoke-WorkspaceSibling {
  [OutputType([void])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  $Location = @{
    Location = $PWD | Microsoft.PowerShell.Management\Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias i.. Shell\Invoke-WorkspaceRelative
function Invoke-WorkspaceRelative {
  [OutputType([void])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  $Location = @{
    Location = $PWD | Microsoft.PowerShell.Management\Split-Path | Microsoft.PowerShell.Management\Split-Path
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias i~ Shell\Invoke-WorkspaceHome
function Invoke-WorkspaceHome {
  [OutputType([void])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  $Location = @{
    Location = $HOME
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias ic Shell\Invoke-WorkspaceCode
function Invoke-WorkspaceCode {
  [OutputType([void])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  $Location = @{
    Location = "$HOME\code"
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias i/ Shell\Invoke-WorkspaceDrive
function Invoke-WorkspaceDrive {
  [OutputType([void])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  $Location = @{
    Location = $PWD.Drive.Root
  }
  Invoke-Workspace @PSBoundParameters @Location @args
}
