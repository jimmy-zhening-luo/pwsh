function Resolve-GitRepository {
  [CmdletBinding()]
  [OutputType([string[]])]
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

$GIT_VERB = (Import-PowerShellDataFile -Path (Join-Path $PSScriptRoot Git-Verb.psd1 -Resolve)).GIT_VERB

<#
.SYNOPSIS
Run a Git command
.DESCRIPTION
This function allows you to run a Git command in a local repository. If no command is specified, it defaults to 'git status'. If no path is specified, it defaults to the current location. For every verb except for 'clone', the function will throw an error if there is no Git repository at the specified path.
.LINK
https://git-scm.com/docs
#>
New-Alias g Git\Invoke-GitRepository
function Invoke-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # Git verb (command) to run
    [string]$Verb,
    # Stop execution on Git error
    [switch]$Throw
  )

  $GitArguments = $args

  if ($Path.StartsWith('-')) {
    $GitArguments = , $Path + $GitArguments
    $Path = ''

    if ($Verb -and -not $Verb.StartsWith('-')) {
      $Path, $Verb = $Verb, ''
    }
  }

  if ($Verb.StartsWith('-')) {
    $GitArguments = , $Verb + $GitArguments
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
  }
  elseif ($Path -and -not $Verb) {
    if (-not (Resolve-GitRepository -Path $Path) -and $Path -in $GIT_VERB) {
      $Verb, $Path = $Path.ToLowerInvariant(), ''
    }
  }
  elseif ($Verb -and -not $Path) {
    if ($Verb -notin $GIT_VERB) {
      throw "Unknown git verb '$Verb'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }

    $Verb = $Verb.ToLowerInvariant()
  }

  $Resolve = @{
    Path = $Path
    New  = $Verb -eq 'clone'
  }
  $Repository = Resolve-GitRepository @Resolve

  if (-not $Repository) {
    throw "'Path '$Path' is not a Git repository"
  }

  if (-not $Verb) {
    $Verb = 'status'
  }

  $GitArguments = @(
    '-c'
    'color.ui=always'
    '-C'
    $Repository
    $Verb
  ) + $GitArguments
  if ($Throw) {
    & git @GitArguments 2>&1 |
      Tee-Object -Variable GitResult

    if ($GitResult -match '^(?>fatal:)') {
      throw $GitResult
    }
  }
  else {
    & git @GitArguments
  }
}
