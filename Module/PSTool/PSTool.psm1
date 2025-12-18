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

  [hashtable]$Private:CodeEdit = @{
    FilePath     = "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = @(
      [string](Get-PSReadLineOption).HistorySavePath
      '--profile=PowerShell'
      '--new-window'
    )
    NoNewWindow  = $True
  }
  Start-Process @CodeEdit
}

[string]$PROFILE_REPO_ROOT = "$REPO_ROOT\pwsh"

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

  [hashtable]$Private:CodeEdit = @{
    FilePath     = "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = @(
      "$PROFILE_REPO_ROOT"
      '--profile=Default'
    )
    NoNewWindow  = $True
  }
  Start-Process @CodeEdit
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
    [ushort]$Iterations,

    [Parameter(
      ParameterSetName = 'Number',
      Mandatory
    )]
    # If specified, returns only the numeric performance value in milliseconds.
    [switch]$Number,

    [Parameter(DontShow)][switch]$zNothing

  )

  if (-not $Iterations) {
    $Iterations = 1
  }

  [double]$Private:StartupLoadProfile = 0
  [double]$Private:NormalStartup = 0

  [hashtable]$Private:Test = @{
    Command = '1'
  }
  for ([ushort]$i = 0; $i -lt $Iterations; ++$i) {
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

<#
.SYNOPSIS
Update PowerShell profile repository.

.DESCRIPTION
This function updates the PowerShell profile repository by pulling the latest changes from the remote Git repository, updating the PSScriptAnalyzer settings file in the user's home directory, and rebuilding the profile's .NET dependencies unless the SkipBuild switch is specified.

.COMPONENT
PSTool
#>
function Update-PSProfile {

  [CmdletBinding()]

  param(

    [Alias('NoBuild')]
    # If specified, skips the build step after pulling the latest changes.
    [switch]$SkipBuild,

    [Parameter(DontShow)][switch]$zNothing

  )

  #region Pull Repo
  [string[]]$GitCommandManifest = @(
    '-c'
    'color.ui=always'
    '-C'
    $PROFILE_ROOT
    'pull'
  )
  & "$env:ProgramFiles\Git\cmd\git.exe" @GitCommandManifest

  if ($LASTEXITCODE -ne 0) {
    throw "Failed to pull pwsh profile repository at '$PROFILE_ROOT'. Git returned exit code: $LASTEXITCODE"
  }
  #endregion


  #region Copy Linter
  [hashtable]$Private:Linter = @{
    Path     = "$PROFILE_ROOT\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Test-Path @Linter) {
    [hashtable]$Private:Copy = @{
      Path        = $Linter.Path
      Destination = $HOME
    }
    Copy-Item @Copy
  }
  #endregion


  #region Build
  if (-not $SkipBuild) {
    [hashtable]$Private:CompileCommand = @{
      All         = $True
      CommandType = 'Application'
      Name        = 'dotnet.exe'
    }
    [System.Management.Automation.ApplicationInfo]$Private:DotnetExecutable = Get-Command @CompileCommand

    if (-not $Private:DotnetExecutable) {
      try {
        [System.Management.Automation.ApplicationInfo]$Private:DotnetExecutable = Install-PSModuleDotnet

        if (-not $Private:DotnetExecutable) {
          throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
        }
      }
      catch {
        throw 'Failed to install Microsoft.DotNet.SDK.10'
      }
    }

    [hashtable]$Private:Dotnet = @{
      FilePath         = (Resolve-Path -Path $Private:DotnetExecutable.Source).Path
      WorkingDirectory = "$PROFILE_REPO_ROOT\Class"
      NoNewWindow      = $True
      PassThru         = $True
      ErrorAction      = 'Stop'
    }

    [string[]]$Private:DotnetClean = @(
      'clean'
      '--configuration'
      'Release'
    )
    Start-Process @Private:Dotnet -ArgumentList $Private:DotnetClean |
      Wait-Process

    [string[]]$Private:DotnetBuild = @(
      'build'
      '--configuration'
      'Release'
    )
    Start-Process @Private:Dotnet -ArgumentList $Private:DotnetBuild |
      Wait-Process
  }
  #endregion
}

function Install-PSModuleDotnet {

  [CmdletBinding(
    SupportsShouldProcess,
    ConfirmImpact = 'High'
  )]

  [OutputType([System.Management.Automation.ApplicationInfo])]

  param()

  begin {
    [string[]]$Private:AppId = @(
      '--id=Microsoft.DotNet.SDK.10'
    )
  }

  process {
    if (
      $PSCmdlet.ShouldProcess(
        $AppId,
        'winget install'
      )
    ) {
      & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe @AppId

      if ($LASTEXITCODE -ne 0) {
        throw 'winget attempted to install Microsoft.DotNet.SDK.10 but returned a non-zero exit code'
      }

      [hashtable]$Private:CompileCommand = @{
        All         = $True
        CommandType = 'Application'
        Name        = 'dotnet.exe'
      }
      [System.Management.Automation.ApplicationInfo]$Private:DotnetExecutable = Get-Command @CompileCommand

      if (-not $Private:DotnetExecutable) {
        throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
      }

      try {
        [hashtable]$Private:DotnetInstallDependency = @{
          FilePath     = (Resolve-Path -Path $Private:DotnetExecutable.Source).Path
          NoNewWindow  = $True
          PassThru     = $True
          ErrorAction  = 'Stop'
          ArgumentList = [string[]]@(
            'new'
            'install'
            'Microsoft.PowerShell.Standard.Module.Template'
          )
        }
        Start-Process @Private:DotnetInstallDependency |
          Wait-Process
      }
      catch {
        throw 'Failed to install required dotnet dependency: Microsoft.PowerShell.Standard.Module.Template'
      }

      return $Private:DotnetExecutable
    }
  }
}

New-Alias oc Invoke-PSHistory
New-Alias op Invoke-PSProfile
New-Alias up Update-PSProfile
New-Alias mc Measure-PSProfile
