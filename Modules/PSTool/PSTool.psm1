New-Alias ^ Select-Object
New-Alias to Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias format Format-Table

New-Alias oc Invoke-PSHistory
function Invoke-PSHistory {
  [OutputType([void])]
  param()

  $Private:History = @{
    Path        = (Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }
  Shell\Invoke-Workspace @History
}

New-Alias op Invoke-PSProfile
function Invoke-PSProfile {
  [OutputType([void])]
  param()

  $Private:ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'PowerShell'
  }
  Shell\Invoke-WorkspaceCode @ProfileRepository @args
}

New-Alias up Update-PSProfile
function Update-PSProfile {
  $Private:ProfileRepository = @{
    Path = Resolve-Path -Path $HOME\code\pwsh
  }
  Shell\Get-GitRepository @ProfileRepository

  Update-PSLinter
}

function Update-PSLinter {
  [OutputType([void])]
  param()

  $Private:Linter = @{
    Path     = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Test-Path @Linter) {
    $Private:Copy = @{
      Path        = $Linter.Path
      Destination = $HOME
    }
    Copy-Item @Copy
  }
}

New-Alias mc Measure-PSProfile
function Measure-PSProfile {
  [OutputType([string])]
  [OutputType([int], ParameterSetName = 'Number')]
  param(
    [Parameter(
      ParameterSetName = 'Number'
    )]
    [switch]$Number
  )

  $Private:Test = @{
    Command = '1'
  }

  [double]$Private:StartupLoadProfile = 0
  [double]$Private:NormalStartup = 0

  $Private:Iterations = 1

  for ($i = 0; $i -lt $Iterations; ++$i) {
    $StartupLoadProfile += (
      Measure-Command { pwsh @Test }
    ).TotalMilliseconds
    $NormalStartup += (
      Measure-Command { pwsh -NoProfile @Test }
    ).TotalMilliseconds
  }

  $Private:Performance = [int][Math]::Max(
    [Math]::Round(
      ($StartupLoadProfile - $NormalStartup) / $Iterations
    ),
    0
  )
  $Private:MeanNormalStartup = [int][Math]::Round($NormalStartup / $Iterations)

  $Number ? $Performance : "$Performance ms`n(Base: $MeanNormalStartup ms)"
}
