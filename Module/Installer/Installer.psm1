$PROFILE_REPO_ROOT = "$REPO_ROOT\pwsh"

function Restore-PSProfile {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('upp')]
  param()

  end {
    Update-PSProfile -Build
  }
}

function Update-PSProfile {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('up')]
  param(

    [switch]$Build
  )

  end {
    $GitCommandManifest = @(
      '-c'
      'color.ui=always'
      '-C'
      $PROFILE_REPO_ROOT
      'pull'
    )
    & "$env:ProgramFiles\Git\cmd\git.exe" @GitCommandManifest

    if ($LASTEXITCODE -notin 0, 1) {
      throw "Failed to pull pwsh profile repository. Git returned exit code: $LASTEXITCODE"
    }

    $LinterConfig = "$PROFILE_REPO_ROOT\Data\PSScriptAnalyzerSettings.psd1"
    if (Test-Path $LinterConfig -PathType Leaf) {
      Copy-Item -Path $LinterConfig -Destination $HOME -Force
    }

    if ($Build) {
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
        (
          Get-Process -Name dotnet
        ).Kill($True)
      }
    }
  }
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

      $DotnetNativeCommand = Get-Command -Name dotnet.exe -CommandType Application -All

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
