Microsoft.PowerShell.Utility\New-Alias e Shell\Invoke-Directory
function Invoke-Directory {
  [OutputType([void], [string[]])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if (-not $Path) {
    $PSBoundParameters.Path = Microsoft.PowerShell.Management\Get-Location
  }

  if ($env:SSH_CLIENT) {
    return Get-File @PSBoundParameters
  }

  $IsFile = @{
    PathType = 'Leaf'
  }
  if (Microsoft.PowerShell.Management\Test-Path @PSBoundParameters @IsFile) {
    [void](Invoke-Workspace @PSBoundParameters)
  }
  else {
    [void](Microsoft.PowerShell.Management\Invoke-Item @PSBoundParameters)
  }
}

Microsoft.PowerShell.Utility\New-Alias e. Shell\Invoke-DirectorySibling
function Invoke-DirectorySibling {
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Invoke-Directory @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias e.. Shell\Invoke-DirectoryRelative
function Invoke-DirectoryRelative {
  param (
    [PathCompletions('..\..')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location | Microsoft.PowerShell.Management\Split-Path | Microsoft.PowerShell.Management\Split-Path) $Path
  }
  Invoke-Directory @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias e~ Shell\Invoke-DirectoryHome
function Invoke-DirectoryHome {
  param (
    [PathCompletions('~')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME $Path
  }
  Invoke-Directory @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias ec Shell\Invoke-DirectoryCode
function Invoke-DirectoryCode {
  param (
    [PathCompletions('~\code')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path $HOME\code $Path
  }
  Invoke-Directory @FullPath @args
}

Microsoft.PowerShell.Utility\New-Alias e/ Shell\Invoke-DirectoryDrive
function Invoke-DirectoryDrive {
  param (
    [PathCompletions('\')]
    [string]$Path
  )

  $FullPath = @{
    Path = Microsoft.PowerShell.Management\Join-Path (Microsoft.PowerShell.Management\Get-Location).Drive.Root $Path
  }
  Invoke-Directory @FullPath @args
}
