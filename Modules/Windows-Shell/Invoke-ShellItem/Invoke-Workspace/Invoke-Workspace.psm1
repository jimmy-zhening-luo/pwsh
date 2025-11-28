New-Alias i Invoke-Workspace
function Invoke-Workspace {
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

  $Local:args = $args

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Local:args = , $Location + $Local:args
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Join-Path $Location $Path) : $Path

    if (Test-Path -Path $Target) {
      $FullPath = Resolve-Path $Target |
        Select-Object -ExpandProperty Path
      $Local:args = , $FullPath + $Local:args
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Target' does not exist."
      }

      if (-not $Location) {
        $Location = '.'
      }

      $Local:args = $Location, $Path + $Local:args
    }
  }
  else {
    if (-not $Location) {
      $Location = '.'
    }

    $Local:args = , $Location + $Local:args
  }

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  if ($ProfileName) {
    if (-not $ProfileName.StartsWith('-')) {
      $Window = $true

      $Local:args += '--profile'
    }

    $Local:args += $ProfileName
  }

  if ($Window) {
    $Local:args += '--new-window'
  }
  elseif ($ReuseWindow) {
    $Local:args += '--reuse-window'
  }

  & code.cmd @Local:args
}

New-Alias i. Invoke-WorkspaceSibling
function Invoke-WorkspaceSibling {
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
    Location = '..'
  }

  Invoke-Workspace @PSBoundParameters @Location @args
}

New-Alias i.. Invoke-WorkspaceRelative
function Invoke-WorkspaceRelative {
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
    Location = '..\..'
  }

  Invoke-Workspace @PSBoundParameters @Location @args
}

New-Alias i~ Invoke-WorkspaceHome
function Invoke-WorkspaceHome {
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
    Location = '~'
  }

  Invoke-Workspace @PSBoundParameters @Location @args
}

New-Alias ic Invoke-WorkspaceCode
function Invoke-WorkspaceCode {
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
    Location = '~\code'
  }

  Invoke-Workspace @PSBoundParameters @Location @args
}

New-Alias i/ Invoke-WorkspaceDrive
function Invoke-WorkspaceDrive {
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
    Location = '\'
  }

  Invoke-Workspace @PSBoundParameters @Location @args
}
