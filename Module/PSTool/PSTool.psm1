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
  [Alias('mcp')]
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
    [switch]$Timespan
  )

  if (-not $Iterations) {
    $Iterations = 8
  }

  Write-Progress -Activity Profiling -Status "0/$Iterations" -PercentComplete 0

  $BareStartupTicks = [System.Collections.Generic.List[long]]::new()
  $StartupWithProfileTicks = [System.Collections.Generic.List[long]]::new()

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

    Write-Progress -Activity Profiling -Status "$i/$Iterations" -PercentComplete ($i * 100 / $Iterations)
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
  [Alias('oc')]
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
  [Alias('op')]
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
  [Alias('up')]
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
  [Alias('upp')]
  param(

    # If specified, skips the build step after syncing the repository and linter.
    [switch]$SkipBuild
  )

  #region Repo
  Write-Progress -Activity 'Update Profile' -Status Pull -PercentComplete 0

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

  #region Linter
  Write-Progress -Activity 'Update Profile' -Status 'Copy Linter' -PercentComplete 30

  $LinterConfig = "$PROFILE_REPO_ROOT\Data\PSScriptAnalyzerSettings.psd1"

  if (Test-Path $LinterConfig -PathType Leaf) {
    Copy-Item -Path $LinterConfig -Destination $HOME -Force
  }
  #endregion

  #region Build
  if (-not $SkipBuild) {
    Write-Progress -Activity 'Update Profile' -Status Prebuild -PercentComplete 40
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
        Write-Progress -Activity 'Update Profile' -Status Clean -PercentComplete 45

        & $DotnetNativeCommand.Source clean $Solution --configuration Release

        if ($LASTEXITCODE -notin 0, 1) {
          throw "dotnet.exe returned a non-zero exit code ($LASTEXITCODE) when trying to clean the project."
        }
      }
      catch {
        throw 'Failed to clean project. ' + $PSItem.Exception
      }

      try {
        Write-Progress -Activity 'Update Profile' -Status Build -PercentComplete 60

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
      Write-Progress -Activity 'Update Profile' -Status Cleanup -PercentComplete 90

      (
        Get-Process -Name dotnet
      ).Kill($True)
    }
  }
  #endregion

  #region Cache
  Write-Progress -Activity 'Update Profile' -Status 'Save Cache' -PercentComplete 95

  $SLNX = Select-Xml -XPath Solution -Path $PROFILE_REPO_ROOT\Class\Class.slnx |
    Select-Object -ExpandProperty Node

  function Expand-PSProject {
    [CmdletBinding()]
    [OutputType([string[]])]
    param(
      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Class
    )
    end {
      return $SLNX |
        Select-Xml -XPath (
          'Folder[@Name="/' + $Class + '/"]'
        ) |
        Select-Object -ExpandProperty Node |
        Select-Object -ExpandProperty Project |
        Select-Object -ExpandProperty Path |
        ForEach-Object {
          $PSItem.Substring(
            $PSItem.LastIndexOf([char]'/') + 1
          )
        } |
        ForEach-Object {
          $PSItem.Remove(
            $PSItem.Length - 7
          )
        }
    }
  }

  @{
    Modules = Expand-PSProject -Class Module
    Types   = Expand-PSProject -Class Type
  } |
    ConvertTo-Json -Compress |
    Set-Content -Path $PROFILE_REPO_ROOT\Class\Class.json -Encoding UTF8
  #endregion

  Write-Progress -Activity 'Update Profile' -Status Finished -PercentComplete 100
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
