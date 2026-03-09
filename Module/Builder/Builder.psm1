function Restore-PSProfile {
  [CmdletBinding()]
  [Alias('upr')]
  [OutputType([void])]
  param()
  end { Update-PSProfile -Build -Restore }
}
function Build-PSProfile {
  [CmdletBinding()]
  [Alias('upp')]
  [OutputType([void])]
  param()
  end { Update-PSProfile -Build }
}
function Update-PSProfile {
  [CmdletBinding()]
  [Alias('up')]
  [OutputType([void])]
  param(
    [Parameter()]
    [switch]$Build,

    [Parameter()]
    [switch]$Restore
  )

  end {
    $PROFILE_REPO_ROOT = "$HOME\code\pwsh"
    $GIT = "$env:ProgramFiles\Git\cmd\git.exe"
    $GitArgument = @(
      '-c'
      'color.ui=always'
      '-C'
      $PROFILE_REPO_ROOT
    )

    if ($Restore) {
      Remove-Item -LiteralPath $PROFILE_REPO_ROOT\Class -Recurse -Force

      & $GIT @GitArgument add .
      & $GIT @GitArgument reset --hard

      if ($LASTEXITCODE -notin 0, 1) {
        throw "Git failed to reset profile repository, with exit code: $LASTEXITCODE"
      }
    }

    & $GIT @GitArgument pull

    if ($LASTEXITCODE -notin 0, 1) {
      throw "Git failed to pull profile repository, with exit code: $LASTEXITCODE"
    }

    if (-not $Build) {
      return
    }

    try {
      $DOTNET = "$env:ProgramFiles\dotnet\dotnet.exe"
      $DotnetArgument = @(
        "$PROFILE_REPO_ROOT\Class.slnx"
        '--configuration=Release'
      )

      & $DOTNET clean @DotnetArgument --verbosity=quiet

      if ($LASTEXITCODE -notin 0, 1) {
        throw "dotnet failed to clean profile project, with exit code: $LASTEXITCODE"
      }

      if ($Restore) {
        $DotnetArgument += @(
          '--force'
          '--disable-build-servers'
          '--no-incremental'
        )
      }

      & $DOTNET build @DotnetArgument

      if ($LASTEXITCODE -notin 0, 1) {
        throw "dotnet failed to build profile project, with exit code: $LASTEXITCODE"
      }
    }
    catch {
      throw
    }
    finally {
      Get-Process -Name dotnet -ErrorAction SilentlyContinue |
        ForEach-Object -MemberName Kill -ArgumentList $true
    }
  }
}
