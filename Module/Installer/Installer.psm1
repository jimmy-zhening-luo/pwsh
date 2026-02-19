function Restore-PSProfile {
  [CmdletBinding()]
  [OutputType([void])]
  [Alias('upr')]
  param()

  end {
    Update-PSProfile -Build -Restore
  }
}

function Build-PSProfile {
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

    [Parameter()]
    [switch]$Build,

    [Parameter()]
    [switch]$Restore
  )

  end {
    $PROFILE_REPO_ROOT = "$HOME\code\pwsh"
    $GitArgument = @(
      '-c'
      'color.ui=always'
      '-C'
      $PROFILE_REPO_ROOT
    )

    if ($Restore) {
      & $env:ProgramFiles\Git\cmd\git.exe @GitArgument reset

      if ($LASTEXITCODE -notin 0, 1) {
        throw "Failed to reset pwsh profile repository. Git returned exit code: $LASTEXITCODE"
      }
    }

    & $env:ProgramFiles\Git\cmd\git.exe @GitArgument pull

    if ($LASTEXITCODE -notin 0, 1) {
      throw "Failed to pull pwsh profile repository. Git returned exit code: $LASTEXITCODE"
    }

    if (-not $Build) {
      return
    }

    try {
      $DotnetExecutable = (
        Get-Command -Name dotnet.exe -CommandType Application -All
      ).Source

      $DotnetArgument = @(
        "$PROFILE_REPO_ROOT\Class.slnx"
        '--configuration=Release'
        '--nologo'
      )

      & $DotnetExecutable clean @DotnetArgument --verbosity=quiet

      if ($LASTEXITCODE -notin 0, 1) {
        throw "dotnet returned a non-zero exit code ($LASTEXITCODE) when trying to clean the project."
      }

      $DotnetBuildArgument = @()

      if ($Restore) {
        $DotnetBuildArgument += @(
          '--force'
          '--no-incremental'
          '--disable-build-servers'
        )
      }

      & $DotnetExecutable build @DotnetArgument @DotnetBuildArgument

      if ($LASTEXITCODE -notin 0, 1) {
        throw "dotnet returned a non-zero exit code ($LASTEXITCODE) when trying to build the project."
      }
    }
    catch {
      throw 'Failed to clean and build project. ' + $PSItem.Exception
    }
    finally {
      $DotnetProcess = Get-Process |
        Where-Object {
          $PSItem.ProcessName -eq 'dotnet'
        }

      if ($null -ne $DotnetProcess) {
        $DotnetProcess.Kill($True)
      }
    }
  }
}
