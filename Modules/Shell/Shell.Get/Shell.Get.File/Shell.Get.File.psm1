#Requires -Modules Microsoft.PowerShell.Management, Microsoft.PowerShell.Utility

Microsoft.PowerShell.Utility\New-Alias p Shell\Get-File
function Get-File {
  [OutputType([string[]])]
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [string]$Location
  )

  $Local:args = $args

  if (
    $Location -and -not (
      Microsoft.PowerShell.Management\Test-Path -Path $Location -PathType Container
    )
  ) {
    $Local:args = , $Location + $Local:args
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Microsoft.PowerShell.Management\Join-Path $Location $Path) : $Path

    if (-not (Microsoft.PowerShell.Management\Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    $FullPath = @{
      Path = Microsoft.PowerShell.Management\Resolve-Path -Path $Target
    }
    $Container = @{
      PathType = 'Container'
    }
    if (Microsoft.PowerShell.Management\Test-Path @FullPath @Container) {
      Microsoft.PowerShell.Management\Get-ChildItem @FullPath @Local:args
    }
    else {
      Microsoft.PowerShell.Management\Get-Content @FullPath @Local:args
    }
  }
  else {
    $Directory = @{
      Path = $Location ? (Microsoft.PowerShell.Management\Resolve-Path -Path $Location) : (Microsoft.PowerShell.Management\Get-Location).Path
    }
    Microsoft.PowerShell.Management\Get-ChildItem @Directory @Local:args
  }
}

Microsoft.PowerShell.Utility\New-Alias p. Shell\Get-FileSibling
function Get-FileSibling {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  $Location = @{
    Location = Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias p.. Shell\Get-FileRelative
function Get-FileRelative {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  $Location = @{
    Location = Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path | Microsoft.PowerShell.Management\Split-Path
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias p~ Shell\Get-FileHome
function Get-FileHome {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  $Location = @{
    Location = $HOME
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias pc Shell\Get-FileCode
function Get-FileCode {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  $Location = @{
    Location = "$HOME\code"
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

Microsoft.PowerShell.Utility\New-Alias p/ Shell\Get-FileDrive
function Get-FileDrive {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  $Location = @{
    Location = (Microsoft.PowerShell.Management\Get-Location).Drive.Root
  }
  Shell\Get-File @PSBoundParameters @Location @args
}
