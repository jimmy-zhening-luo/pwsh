using namespace System.Collections.Generic
using namespace System.Management.Automation

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
    FilePath     = "$HOME\AppData\Local\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = @(
      [string](Get-PSReadLineOption).HistorySavePath
      '--profile=PowerShell'
      '--new-window'
    )
    NoNewWindow  = $True
  }
  Start-Process @CodeEdit
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

  [hashtable]$Private:CodeEdit = @{
    FilePath     = "$HOME\AppData\Local\Programs\Microsoft VS Code\bin\code.cmd"
    ArgumentList = @(
      [string](Get-PSReadLineOption).HistorySavePath
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

  $Private:PROFILE_ROOT = "$HOME\code\pwsh"

  #region Git Pull
  [string[]]$GitCommandManifest = @(
    '-c'
    'color.ui=always'
    '-C'
    $PROFILE_ROOT
    'pull'
  )
  & 'C:\Program Files\Git\cmd\git.exe' @GitCommandManifest

  if ($LASTEXITCODE -ne 0) {
    throw "Failed to pull pwsh profile repository at '$PROFILE_ROOT'. Git returned exit code: $LASTEXITCODE"
  }
  #endregion

  #region Linter
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
  [string]$Private:CLASS_ROOT = "$PROFILE_ROOT\Class"
  [hashtable]$Private:CLASSES = Import-PowerShellDataFile -Path $PROFILE_ROOT\Data\Class.psd1
  [string[]]$Projects = $CLASSES.Types + $CLASSES.Modules

  $Private:Modified = [List[string]]::new()

  foreach ($Private:Project in $Projects) {
    [hashtable]$Private:Built = @{
      Path = "$CLASS_ROOT\$Project\bin\Release\netstandard2.0\$Project.dll"
    }
    [hashtable]$Private:Source = @{
      Path = "$CLASS_ROOT\$Project\$Project.cs"
    }
    if (
      -not (
        Test-Path @Built -PathType Leaf
      ) -or (
        Get-Item @Built
      ).LastWriteTime -lt (
        Get-Item @Source
      ).LastWriteTime
    ) {
      $Modified.Add([string]$Project)
    }
  }

  if ($Modified.Count -ne 0) {
    Build-PSProfile
  }
  #endregion
}

function Build-PSProfile {

  [CmdletBinding()]

  [OutputType([void])]

  param()

  [hashtable]$Private:CompileCommand = @{
    All         = $True
    CommandType = 'Application'
    Name        = 'dotnet.exe'
  }
  [ApplicationInfo]$Private:DotNetExecutable = Get-Command @CompileCommand

  if (-not $DotNetExecutable) {
    try {
      [ApplicationInfo]$Private:DotNetExecutable = Install-PSModuleDotNet

      if (-not $DotNetExecutable) {
        throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
      }
    }
    catch {
      throw 'Failed to install Microsoft.DotNet.SDK.10'
    }
  }

  [hashtable]$Private:DotNet = @{
    FilePath         = (Resolve-Path -Path $DotNetExecutable.Source).Path
    WorkingDirectory = "$HOME\code\pwsh"
    NoNewWindow      = $True
    PassThru         = $True
    ErrorAction      = 'Stop'
  }

  $Private:DotNetClean = [List[string]]::new(
    [string[]]@(
      'clean'
      '--configuration'
      'Release'
    )
  )
  Start-Process @DotNet -ArgumentList $DotNetClean |
    Wait-Process

  $Private:DotNetBuild = [List[string]]::new(
    [string[]]@(
      'build'
      '--configuration'
      'Release'
    )
  )
  Start-Process @DotNet -ArgumentList $DotNetBuild |
    Wait-Process
}

function Install-PSModuleDotNet {

  [CmdletBinding(
    SupportsShouldProcess,
    ConfirmImpact = 'High'
  )]

  [OutputType([System.Management.Automation.ApplicationInfo])]

  param()

  begin {
    $Private:AppId = [List[string]]::new(
      [List[string]]@(
        '--id=Microsoft.DotNet.SDK.10'
      )
    )
  }

  process {
    if (
      $PSCmdlet.ShouldProcess(
        $AppId.ToArray(),
        'winget install'
      )
    ) {
      & "$HOME\AppData\Local\Microsoft\WindowsApps\winget.exe" @AppId

      if ($LASTEXITCODE -ne 0) {
        throw 'winget attempted to install Microsoft.DotNet.SDK.10 but returned a non-zero exit code'
      }

      [hashtable]$Private:CompileCommand = @{
        All         = $True
        CommandType = 'Application'
        Name        = 'dotnet.exe'
      }
      [ApplicationInfo]$Private:DotNetExecutable = Get-Command @CompileCommand

      if (-not $DotNetExecutable) {
        throw 'Failed to locate Microsoft.DotNet.SDK.10 executable post-installation'
      }

      try {
        [hashtable]$Private:DotNetInstallDependency = @{
          FilePath     = (Resolve-Path -Path $DotNetExecutable.Source).Path
          NoNewWindow  = $True
          PassThru     = $True
          ErrorAction  = 'Stop'
          ArgumentList = [List[string]]::new(
            [string[]]@(
              'new'
              'install'
              'Microsoft.PowerShell.Standard.Module.Template'
            )
          )
        }
        Start-Process @DotNetInstallDependency |
          Wait-Process
      }
      catch {
        throw 'Failed to install required dotnet dependency: Microsoft.PowerShell.Standard.Module.Template'
      }

      return $DotNetExecutable
    }
  }
}

New-Alias oc Invoke-PSHistory
New-Alias op Invoke-PSProfile
New-Alias up Update-PSProfile
New-Alias mc Measure-PSProfile
