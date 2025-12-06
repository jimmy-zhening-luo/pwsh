New-Alias ^ Select-Object
New-Alias pick Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias n Measure-Object
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias format Format-Table

New-Alias oc PSTool\Invoke-PSHistory
function Invoke-PSHistory {
  [OutputType([void])]
  param()

  $History = @{
    Path        = (Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }
  [void](Shell\Invoke-Workspace @History)
}

New-Alias op PSTool\Invoke-PSProfile
function Invoke-PSProfile {
  [OutputType([void])]
  param()

  $ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'PowerShell'
  }
  [void](Shell\Invoke-WorkspaceCode @ProfileRepository @args)
}

New-Alias up PSTool\Update-PSProfile
function Update-PSProfile {
  $ProfileRepository = @{
    Path = Join-Path $HOME code\pwsh -Resolve
  }
  Git\Get-Repository @ProfileRepository
  Update-PSLinter
}

function Update-PSLinter {
  [OutputType([void])]
  param()

  $Linter = @{
    Path     = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Test-Path @Linter) {
    $Copy = @{
      Path        = $Linter.Path
      Destination = $HOME
    } 
    [void](Copy-Item @Copy)
  }
}

New-Alias mc PSTool\Measure-PSProfile
function Measure-PSProfile {
  [OutputType([string])]
  [OutputType([int], ParameterSetName='Number')]
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
      Measure-Command { pwsh @Test }
    ).TotalMilliseconds
    $NormalStartup += (
      Measure-Command { pwsh -NoProfile @Test }
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
