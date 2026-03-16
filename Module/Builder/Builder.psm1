function Restore-PSProfile {
  [Alias('upr')]
  param()

  Update-PSProfile -Build -Restore
}
function Build-PSProfile {
  [Alias('upp')]
  param()

  Update-PSProfile -Build
}
function Update-PSProfile {
  [Alias('up')]
  param(
    [Parameter()]
    [switch]$Build,

    [Parameter()]
    [switch]$Restore
  )

  $PROFILE_REPO_ROOT = "$HOME\code\pwsh"
  $GIT = "$env:ProgramFiles\Git\cmd\git.exe"
  $GitArgument = @(
    '-c'
    'color.ui=always'
    '-C'
    $PROFILE_REPO_ROOT
  )

  if ($Restore) {
    Remove-Item -LiteralPath $PROFILE_REPO_ROOT\Build -Recurse -Force

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

    if ($Restore) {
      & $DOTNET clean @DotnetArgument --verbosity=quiet

      if ($LASTEXITCODE -notin 0, 1) {
        throw "dotnet failed to clean profile project, with exit code: $LASTEXITCODE"
      }

      $DotnetArgument += @(
        '--no-incremental'
        '--disable-build-servers'
      )
    }

    & $DOTNET build @DotnetArgument --force

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
