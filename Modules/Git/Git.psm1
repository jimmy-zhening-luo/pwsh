function Resolve-Repository {
  [OutputType([string[]])]
  [CmdletBinding()]
  param(
    [Parameter(
      Mandatory,
      Position = 0,
      ValueFromPipeline
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [string[]]$Path,
    [switch]$New
  )

  begin {
    $CODE = "$HOME\code"
    $Repositories = @()
  }

  process {
    $RepoPath = @{
      Path = $Path
    }
    $Repository = ''

    if ($New) {
      if (Shell\Test-Item @RepoPath) {
        $Repository = Shell\Resolve-Item @RepoPath
      }
      else {
        $RepoPath.Location = $CODE
        $RepoPath.New = $True

        if (Shell\Test-Item @RepoPath) {
          $Repository = Shell\Resolve-Item @RepoPath
        }
      }
    }
    else {
      $RepoGitPath = @{
        Path           = $Path ? (Join-Path $Path .git) : '.git'
        RequireSubpath = $True
      }

      if (Shell\Test-Item @RepoGitPath) {
        $Repository = Shell\Resolve-Item @RepoPath
      }
      else {
        $RepoGitPath.Location = $RepoPath.Location = $CODE

        if (Shell\Test-Item @RepoGitPath) {
          $Repository = Shell\Resolve-Item @RepoPath
        }
      }
    }

    if ($Repository) {
      $Repositories += $Repository
    }
  }

  end {
    $Repositories
  }
}

$GIT_VERB_FILE = @{
  Path = Join-Path $PSScriptRoot Git-Verb.psd1 -Resolve
  ErrorAction = 'Stop'
}
$GIT_VERB = Import-PowerShellDataFile @GIT_VERB_FILE |
  Select-Object -ExpandProperty GIT_VERB

New-Alias gg Git\Invoke-Repository
function Invoke-Repository {
  param(
    [string]$Path,
    [string]$Verb,
    [switch]$Throw
  )

  $Local:args = $args
  $GitArguments = @(
    '-c'
    'color.ui=always'
    '-C'
  )

  if ($Path.StartsWith('-')) {
    $Local:args = , $Path + $Local:args
    $Path = ''

    if ($Verb -and -not $Verb.StartsWith('-')) {
      $Path, $Verb = $Verb, ''
    }
  }

  if ($Verb.StartsWith('-')) {
    $Local:args = , $Verb + $Local:args
    $Verb = ''
  }

  if ($Path -and $Verb) {
    if ($Verb -in $GIT_VERB) {
      $Verb = $Verb.ToLowerInvariant()
    }
    elseif ($Path -in $GIT_VERB) {
      $Verb, $Path = $Path.ToLowerInvariant(), ''
    }
    else {
      throw "Unknown git verb '$Verb' or '$Path'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }

    $Resolve = @{
      Path = $Path
      New  = $Verb -eq 'clean'
    }
    $Repository = Resolve-Repository @Resolve
  }
  elseif ($Path -and -not $Verb) {
    $Resolve = @{
      Path = $Path
    }
    $Repository = Resolve-Repository @Resolve

    if ($Repository) {
      $Verb = 'status'
    }
    elseif ($Path -in $GIT_VERB) {
      $Verb, $Path = $Path.ToLowerInvariant(), ''

      $Resolve = @{
        Path = $Path
        New = $Verb -eq 'clean'
      }

      $Repository = Resolve-Repository @Resolve
    }
  }
  elseif ($Verb -and -not $Path) {
    if ($Verb -notin $GIT_VERB) {
      throw "Unknown git verb '$Verb'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }

    $Path, $Verb = '', $Verb.ToLowerInvariant()
    $Resolve = @{
      Path = $Path
      New  = $Verb -eq 'clean'
    }
    $Repository = Resolve-Repository @Resolve
  }
  else {
    $Path = ''
    $Resolve = @{
      Path = $Path
    }
    $Repository = Resolve-Repository @Resolve

    if ($Repository) {
      $Verb = 'status'
    }
  }

  if (-not $Repository -or -not $Verb) {
    throw "'git $Verb' requires an existing repository. The path '$Path' could not be resolved to any repository."
  }

  $GitArguments += $Repository, $Verb

  if ($Throw) {
    $GitOutput = ''

    & git $GitArguments @Local:args 2>&1 |
      Tee-Object -Variable GitOutput

    if (($GitOutput -as [string]).StartsWith('fatal:')) {
      throw $GitOutput
    }
  }
  else {
    & git $GitArguments @Local:args
  }
}
