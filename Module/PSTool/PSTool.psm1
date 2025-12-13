<#
.SYNOPSIS
Open PowerShell command history in a text editor.

.DESCRIPTION
This function opens the PowerShell command history file in Visual Studio Code.

.COMPONENT
PSTool
#>
function Invoke-PSHistory {

  [CmdletBinding()]

  [OutputType([void])]

  param()

  [hashtable]$Private:History = @{
    Path        = (Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }
  Shell\Invoke-Workspace @History
}

<#
.SYNOPSIS
Open PowerShell profile repository.

.DESCRIPTION
This function opens the PowerShell profile repository in Visual Studio Code.

.COMPONENT
PSTool
#>
function Invoke-PSProfile {

  [CmdletBinding()]

  [OutputType([void])]

  param()

  [hashtable]$Private:ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'Default'
  }
  Shell\Invoke-WorkspaceCode @ProfileRepository
}

function Build-PSProfile {

  [CmdletBinding()]

  [OutputType([void])]

  param()

  [hashtable]$Private:Build = @{
    FilePath         = 'C:\Program Files\dotnet\dotnet.exe'
    ArgumentList     = 'publish'
    WorkingDirectory = "$HOME\code\pwsh"
    NoNewWindow      = $True
    Wait             = $True
    PassThru         = $True
  }
  Start-Process @Build | Wait-Process
}

<#
.SYNOPSIS
Update PowerShell profile repository.

.DESCRIPTION
This function updates the PowerShell profile repository by pulling the latest changes from the remote Git repository and updating the PSScriptAnalyzer settings file in the user's home directory.

.COMPONENT
PSTool
#>
function Update-PSProfile {

  [CmdletBinding()]

  param()

  $Private:ProfileRepository = Resolve-Path -Path $HOME\code\pwsh

  [hashtable]$Private:Pull = @{
    WorkingDirectory = $ProfileRepository
  }
  Shell\Get-GitRepository @Pull

  Update-PSLinter

  [hashtable]$Private:Compiled = @{
    Path = "$ProfileRepository\Cmdlet\Good\bin\Release\net10.0\Good.dll"
  }
  [hashtable]$Private:Source = @{
    Path = "$ProfileRepository\Cmdlet\Good\Good.cs"
  }
  if (
    -not (Test-Path @Compiled) -or (
      Get-Item @Source
    ).LastWriteTime -gt (
      Get-Item @Compiled
    ).LastWriteTime
  ) {
    Build-PSProfile
  }
}

function Update-PSLinter {

  [CmdletBinding()]

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

<#
.SYNOPSIS
Measure PowerShell profile load time.

.DESCRIPTION
This function measures the load time of the PowerShell profile by comparing the startup time with and without the profile loaded. It performs multiple iterations to calculate an average load time.

.COMPONENT
PSTool
#>
function Measure-PSProfile {

  [CmdletBinding(
    DefaultParameterSetName = 'String'
  )]

  [OutputType([string])]

  [OutputType([int], ParameterSetName = 'Number')]

  param(

    [Parameter(
      ParameterSetName = 'String',
      Position = 0
    )]
    [Parameter(
      ParameterSetName = 'Number',
      Position = 0
    )]
    [ValidateRange(1, 50)]
    # The number of iterations to perform, maximum 50. Default is 1.
    [UInt16]$Iterations,

    [Parameter(
      ParameterSetName = 'Number',
      Mandatory
    )]
    # If specified, returns only the numeric performance value in milliseconds.
    [switch]$Number

  )

  if (-not $Iterations) {
    $Iterations = 1
  }

  [double]$Private:StartupLoadProfile = 0
  [double]$Private:NormalStartup = 0

  [hashtable]$Private:Test = @{
    Command = '1'
  }
  for ([UInt16]$i = 0; $i -lt $Iterations; ++$i) {
    $StartupLoadProfile += [double](
      Measure-Command { pwsh @Test }
    ).TotalMilliseconds
    $NormalStartup += [double](
      Measure-Command { pwsh -NoProfile @Test }
    ).TotalMilliseconds
  }

  [int]$Private:Performance = [System.Math]::Max(
    [int][System.Math]::Round(
      ($StartupLoadProfile - $NormalStartup) / $Iterations
    ),
    0
  )
  [int]$Private:MeanNormalStartup = [System.Math]::Round($NormalStartup / $Iterations)

  if ($Number) {
    return $Performance
  }
  else {
    return "$Performance ms`n(Base: $MeanNormalStartup ms)"
  }
}

New-Alias ^ Select-Object
New-Alias to Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias format Format-Table

New-Alias oc Invoke-PSHistory
New-Alias op Invoke-PSProfile
New-Alias up Update-PSProfile
New-Alias mc Measure-PSProfile
