Microsoft.PowerShell.Utility\New-Alias ^ Microsoft.PowerShell.Utility\Select-Object
Microsoft.PowerShell.Utility\New-Alias to Microsoft.PowerShell.Utility\Select-Object
Microsoft.PowerShell.Utility\New-Alias k Microsoft.PowerShell.Utility\Get-Member
Microsoft.PowerShell.Utility\New-Alias key Microsoft.PowerShell.Utility\Get-Member
Microsoft.PowerShell.Utility\New-Alias count Microsoft.PowerShell.Utility\Measure-Object
Microsoft.PowerShell.Utility\New-Alias z Microsoft.PowerShell.Utility\Sort-Object
Microsoft.PowerShell.Utility\New-Alias format Microsoft.PowerShell.Utility\Format-Table

Microsoft.PowerShell.Utility\New-Alias oc PSTool\Invoke-PSHistory
function Invoke-PSHistory {
  [OutputType([void])]
  param()

  $History = @{
    Path        = (PSReadLine\Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }
  [void](Shell\Invoke-Workspace @History)
}

Microsoft.PowerShell.Utility\New-Alias op PSTool\Invoke-PSProfile
function Invoke-PSProfile {
  [OutputType([void])]
  param()

  $ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'PowerShell'
  }
  [void](Shell\Invoke-WorkspaceCode @ProfileRepository @args)
}

Microsoft.PowerShell.Utility\New-Alias up PSTool\Update-PSProfile
function Update-PSProfile {
  $ProfileRepository = @{
    Path = Microsoft.PowerShell.Management\Resolve-Path -Path $HOME\code\pwsh
  }
  Git\Get-GitRepository @ProfileRepository

  [void](PSTool\Update-PSLinter)
}

function Update-PSLinter {
  [OutputType([void])]
  param()

  $Linter = @{
    Path     = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Microsoft.PowerShell.Management\Test-Path @Linter) {
    $Copy = @{
      Path        = $Linter.Path
      Destination = $HOME
    }
    [void](Microsoft.PowerShell.Management\Copy-Item @Copy)
  }
}

Microsoft.PowerShell.Utility\New-Alias mc PSTool\Measure-PSProfile
function Measure-PSProfile {
  [OutputType([string])]
  [OutputType([int], ParameterSetName = 'Number')]
  param(
    [Parameter(
      ParameterSetName = 'Number'
    )]
    [switch]$Number
  )

  $Test = @{
    Command = '1'
  }

  [double]$StartupLoadProfile = 0
  [double]$NormalStartup = 0

  $Iterations = 1

  for ($i = 0; $i -lt $Iterations; ++$i) {
    $StartupLoadProfile += (
      Microsoft.PowerShell.Utility\Measure-Command { pwsh @Test }
    ).TotalMilliseconds
    $NormalStartup += (
      Microsoft.PowerShell.Utility\Measure-Command { pwsh -NoProfile @Test }
    ).TotalMilliseconds
  }

  $Performance = [int][Math]::Max(
    [Math]::Round(
      ($StartupLoadProfile - $NormalStartup) / $Iterations
    ),
    0
  )
  $MeanNormalStartup = [int][Math]::Round($NormalStartup / $Iterations)

  $Number ? $Performance : "$Performance ms`n(Base: $MeanNormalStartup ms)"
}
