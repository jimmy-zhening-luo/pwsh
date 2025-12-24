using namespace System.Collections.Generic

<#
.SYNOPSIS
Measure PowerShell command performance.

.DESCRIPTION
This function measures the performance of a PowerShell command by invoking it in a new PowerShell process and comparing its execution time over the PowerShell process invocation time as a baseline.

The invoked PowerShell process will not load the user profile, so any commands or modules defined in the profile will not be available during measurement.

.COMPONENT
PSTool

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/measure-command

.LINK
Measure-Command
#>
function Measure-Performance {
  [CmdletBinding(
    DefaultParameterSetName = 'Display'
  )]
  [OutputType([string])]
  [OutputType(
    [double],
    ParameterSetName = 'Numeric'
  )]
  [OutputType(
    [timespan],
    ParameterSetName = 'Timespan'
  )]
  param(

    [Parameter(
      ParameterSetName = 'Display',
      Mandatory,
      Position = 0,
      ValueFromRemainingArguments
    )]
    [Parameter(
      ParameterSetName = 'Numeric',
      Mandatory,
      Position = 0,
      ValueFromRemainingArguments
    )]
    [Parameter(
      ParameterSetName = 'Timespan',
      Mandatory,
      Position = 0,
      ValueFromRemainingArguments
    )]
    # The PowerShell command to measure. The command must be available in a new PowerShell process without loading the user profile. Multiple strings will be concatenated with spaces.
    [string[]]$Command,

    [Parameter(
      ParameterSetName = 'Numeric',
      Mandatory
    )]
    # If specified, returns only the numeric command performance in milliseconds. Cannot be specified with Timespan.
    [switch]$Numeric,

    [Parameter(
      ParameterSetName = 'Timespan',
      Mandatory
    )]
    # If specified, returns only the command performance as a timespan. Cannot be specified with Numeric.
    [switch]$Timespan,

    [Parameter(DontShow)][switch]$z
  )

  [string]$FullCommand = $Command -join ' '

  [long]$CommandTicks = (
    Measure-Command {
      pwsh -NoProfile -Command "$FullCommand"
    }
  ).Ticks

  $AverageBaseline = Measure-PSProfile -Iterations 5 -Baseline -Timespan

  [long]$CommandCostTicks = $CommandTicks - $AverageBaseline.Ticks

  $CommandCost = [timespan]::new(
    $CommandCostTicks
  )

  if ($Numeric) {
    return $CommandCost.TotalMilliseconds
  }
  elseif ($Timespan) {
    return $CommandCost
  }
  else {
    return "$([long]$CommandCost.TotalMilliseconds) ms`n(Base: $([long]$AverageBaseline.TotalMilliseconds) ms)"
  }
}


<#
.SYNOPSIS
Measure PowerShell profile load overhead.

.DESCRIPTION
This function measures the load overhead of the PowerShell profile by comparing PowerShell startup time with and without loading the profile.

It can perform multiple iterations to calculate a mean load overhead.

.COMPONENT
PSTool

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/measure-command

.LINK
Measure-Command
#>
function Measure-PSProfile {
  [CmdletBinding(
    DefaultParameterSetName = 'Display'
  )]
  [OutputType([string])]
  [OutputType(
    [double],
    ParameterSetName = 'Numeric'
  )]
  [OutputType(
    [timespan],
    ParameterSetName = 'Timespan'
  )]
  param(

    [Parameter(
      ParameterSetName = 'Display',
      Position = 0
    )]
    [Parameter(
      ParameterSetName = 'Numeric',
      Position = 0
    )]
    [Parameter(
      ParameterSetName = 'Timespan',
      Position = 0
    )]
    [ValidateRange(1, 50)]
    # The number of iterations to perform, maximum 50. Default is 8.
    [int]$Iterations,

    [Parameter(
      ParameterSetName = 'Numeric'
    )]
    [Parameter(
      ParameterSetName = 'Timespan'
    )]
    # If specified along with Numeric or with Timespan, returns the baseline instead of the profile overhead.
    [switch]$Baseline,

    [Parameter(
      ParameterSetName = 'Numeric',
      Mandatory
    )]
    # If specified, returns only the numeric profile overhead in milliseconds. Cannot be specified with Timespan.
    [switch]$Numeric,

    [Parameter(
      ParameterSetName = 'Timespan',
      Mandatory
    )]
    # If specified, returns only the profile overhead as a timespan. Cannot be specified with Numeric.
    [switch]$Timespan,

    [Parameter(DontShow)][switch]$z
  )

  if (-not $Iterations) {
    $Iterations = 8
  }

  $BareStartupTicks = [List[long]]::new()
  $StartupWithProfileTicks = [List[long]]::new()

  for (
    $i = 0
    $i -lt $Iterations
    ++$i
  ) {
    if (-not $Baseline) {
      $Command1 = Get-Random 500
      $StartupWithProfileTicks.Add(
        [long](
          Measure-Command {
            pwsh -Command "$Command1"
          }
        ).Ticks
      )
    }

    $Command2 = Get-Random 500
    $BareStartupTicks.Add(
      [long](
        Measure-Command {
          pwsh -NoProfile -Command "$Command2"
        }
      ).Ticks
    )
  }

  [long]$TotalBareStartupTicks = [System.Linq.Enumerable]::Sum(
    $BareStartupTicks
  )
  [long]$AverageBareStartupTicks = $TotalBareStartupTicks / $Iterations
  $AverageBareStartup = [timespan]::new(
    $AverageBareStartupTicks
  )

  if ($Baseline) {
    switch ($PSCmdlet.ParameterSetName) {
      Timespan {
        if ($Timespan) {
          return $AverageBareStartup
        }
      }
      Numeric {
        return $AverageBareStartup.TotalMilliseconds
      }
    }
  }
  else {
    [long]$TotalStartupWithProfileTicks = [System.Linq.Enumerable]::Sum(
      $StartupWithProfileTicks
    )

    [long]$TotalProfileCostTicks = $TotalStartupWithProfileTicks - $TotalBareStartupTicks
    [long]$AverageProfileCostTicks =
    $TotalProfileCostTicks / $Iterations
    $AverageProfileCost = [timespan]::new(
      $AverageProfileCostTicks
    )

    if ($Numeric) {
      return $AverageProfileCost.TotalMilliseconds
    }
    elseif ($Timespan) {
      return $AverageProfileCost
    }
    else {
      return "$([long]$AverageProfileCost.TotalMilliseconds) ms`n(Base: $([long]$AverageBareStartup.TotalMilliseconds) ms)"
    }
  }
}

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

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList (Get-PSReadLineOption).HistorySavePath, --profile=Setting, --new-window
}

$PROFILE_REPO_ROOT = "$REPO_ROOT\pwsh"

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

  Start-Process -FilePath "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd" -NoNewWindow -ArgumentList $PROFILE_REPO_ROOT, --profile=Default
}

<#
.SYNOPSIS
Update the local PowerShell profile repository.

.DESCRIPTION
This function updates the local PowerShell profile repository by invoking Publish-PSProfile, which pulls the latest changes from the remote Git repository and updating the PSScriptAnalyzer settings file in the user's home directory. It specifies the SkipBuild flag, thereby skipping the build step.

.COMPONENT
PSTool
#>
function Update-PSProfile {

  [CmdletBinding()]
  [OutputType([void])]
  param(
    [Parameter(DontShow)][switch]$z
  )

  Publish-PSProfile -SkipBuild
}

<#
.SYNOPSIS
Update and publish the local PowerShell profile repository.

.DESCRIPTION
This function updates the local PowerShell profile repository by pulling the latest changes from the remote Git repository and updating the PSScriptAnalyzer settings file in the user's home directory. It also locally publishes the profile by rebuilding its .NET assemblies unless the SkipBuild switch is specified.

.COMPONENT
PSTool
#>
function Publish-PSProfile {

  [CmdletBinding()]
  [OutputType([void])]
  param(

    [Alias('NoBuild')]
    # If specified, skips the build step after syncing the repository and linter.
    [switch]$SkipBuild,

    [Parameter(DontShow)][switch]$z
  )

  #region Pull Repo
  $GitCommandManifest = @(
    '-c'
    'color.ui=always'
    '-C'
    $PROFILE_REPO_ROOT
    'pull'
  )
  & "$env:ProgramFiles\Git\cmd\git.exe" @GitCommandManifest

  if ($LASTEXITCODE -notin 0, 1) {
    throw "Failed to pull pwsh profile repository at '$PROFILE_REPO_ROOT'. Git returned exit code: $LASTEXITCODE"
  }
  #endregion


  #region Copy Linter
  $LinterConfig = "$PROFILE_REPO_ROOT\Data\PSScriptAnalyzerSettings.psd1"

  if (Test-Path $LinterConfig -PathType Leaf) {
    Copy-Item -Path $LinterConfig -Destination $HOME -Force
  }
  #endregion


  #region Build
  if (-not $SkipBuild) {
    [System.Management.Automation.ApplicationInfo]$DotnetNativeCommand = Get-Command -Name dotnet.exe -CommandType Application -All

    if (-not $DotnetNativeCommand) {
      try {
        [System.Management.Automation.ApplicationInfo]$DotnetNativeCommand = Install-PSModuleDotnet

        if (-not $DotnetNativeCommand) {
          throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
        }
      }
      catch {
        throw 'Failed to install Microsoft.DotNet.SDK.10' + $PSItem.Exception
      }
    }

    $Solution = "$PROFILE_REPO_ROOT\Class\Class.slnx"

    try {
      try {
        & $DotnetNativeCommand.Source clean $Solution --configuration Release

        if ($LASTEXITCODE -notin 0, 1) {
          throw "dotnet.exe returned a non-zero exit code ($LASTEXITCODE) when trying to clean the project."
        }
      }
      catch {
        throw 'Failed to clean project. ' + $PSItem.Exception
      }

      try {
        & $DotnetNativeCommand.Source build $Solution --configuration Release

        if ($LASTEXITCODE -notin 0, 1) {
          throw "dotnet.exe returned a non-zero exit code ($LASTEXITCODE) when trying to build the project."
        }
      }
      catch {
        throw 'Failed to clean project. ' + $PSItem.Exception
      }
    }
    catch {
      throw 'Failed to build profile project. ' + $PSItem.Exception
    }
    finally {
      Stop-Process -Name dotnet -Force
    }
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

  process {
    if (
      $PSCmdlet.ShouldProcess(
        '--id=Microsoft.DotNet.SDK.10',
        'winget install'
      )
    ) {
      & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe '--id=Microsoft.DotNet.SDK.10'

      if ($LASTEXITCODE -notin 0, 1) {
        throw "winget attempted to install Microsoft.DotNet.SDK.10 but returned a non-zero exit code ($LASTEXITCODE)"
      }

      [System.Management.Automation.ApplicationInfo]$DotnetNativeCommand = Get-Command -Name dotnet.exe -CommandType Application -All

      if (-not $DotnetNativeCommand) {
        throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
      }

      try {
        & $DotnetNativeCommand.Source new install Microsoft.PowerShell.Standard.Module.Template

        if ($LASTEXITCODE -notin 0, 1) {
          throw "dotnet.exe returned a non-zero exit code ($LASTEXITCODE) when trying to install Microsoft.PowerShell.Standard.Module.Template"
        }
      }
      catch {
        throw 'Failed to install required dotnet dependency: Microsoft.PowerShell.Standard.Module.Template ' + $PSItem.Exception
      }

      return $DotnetNativeCommand
    }
  }
}

New-Alias mc Measure-Performance
New-Alias mcp Measure-PSProfile
New-Alias oc Invoke-PSHistory
New-Alias op Invoke-PSProfile
New-Alias up Update-PSProfile
New-Alias upp Publish-PSProfile
