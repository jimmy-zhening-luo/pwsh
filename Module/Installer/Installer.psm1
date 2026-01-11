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
    $PROFILE_REPO_ROOT = "$REPO_ROOT\pwsh"
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

    Copy-Item -Path $PROFILE_REPO_ROOT\Data\PSScriptAnalyzerSettings.psd1 -Destination $HOME -Force

    if ($Build) {
      $DotnetNativeCommand = Get-Command -Name dotnet.exe -CommandType Application -All

      if (-not $DotnetNativeCommand) {
        throw 'Microsoft.DotNet.SDK.10 is not installed'
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
          throw 'Failed to build project. ' + $PSItem.Exception
        }
      }
      catch {
        throw $PSItem.Exception
      }
      finally {
        (
          Get-Process -Name dotnet
        ).Kill($True)
      }
    }
  }
}
