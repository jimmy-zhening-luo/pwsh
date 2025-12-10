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

  [hashtable]$Private:History = @{
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

  [hashtable]$Private:ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'PowerShell'
  }
  Shell\Invoke-WorkspaceCode @ProfileRepository @args
}

New-Alias up Update-PSProfile
function Update-PSProfile {
  [hashtable]$Private:ProfileRepository = @{
    Path = (Resolve-Path -Path $HOME\code\pwsh).Path
  }
  Shell\Get-GitRepository @ProfileRepository

  Update-PSLinter
}

function Update-PSLinter {
  [OutputType([void])]
  param()

  [hashtable]$Private:Linter = @{
    Path     = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Test-Path @Linter) {
    [hashtable]$Private:Copy = @{
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

  [hashtable]$Private:Test = @{
    Command = '1'
  }

  [double]$Private:StartupLoadProfile = 0
  [double]$Private:NormalStartup = 0

  [UInt16]$Private:Iterations = 1

  for ([UInt16]$Private:i = 0; $i -lt $Iterations; ++$i) {
    $StartupLoadProfile += [double](
      Measure-Command { pwsh @Test }
    ).TotalMilliseconds
    $NormalStartup += [double](
      Measure-Command { pwsh -NoProfile @Test }
    ).TotalMilliseconds
  }

  [int]$Private:Performance = [Math]::Max(
    [int][Math]::Round(
      ($StartupLoadProfile - $NormalStartup) / $Iterations
    ),
    0
  )
  [int]$Private:MeanNormalStartup = [Math]::Round($NormalStartup / $Iterations)

  if ($Number) {
    return $Performance
  }
  else {
    return "$Performance ms`n(Base: $MeanNormalStartup ms)"
  }
}
