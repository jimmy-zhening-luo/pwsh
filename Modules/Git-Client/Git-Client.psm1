$GIT_VERB = (
  Import-PowerShellDataFile (
    Join-Path $PSScriptRoot "Git-Verb.psd1" -Resolve
  ) -ErrorAction Stop
).GIT_VERB

$GitVerbArgumentCompleter = {
  param (
    $commandName,
    $parameterName,
    $wordToComplete,
    $commandAst,
    $fakeBoundParameters
  )

  $GIT_VERB |
    ? {
      $_ -like "$wordToComplete*"
    }
}
Register-ArgumentCompleter -CommandName Invoke-Repository -ParameterName Verb -ScriptBlock $GitVerbArgumentCompleter

function Resolve-Repository {
  param(
    [System.String]$Path,
    [Alias("Clone")]
    [switch]$Initialize
  )

  $Container = @{
    PathType = "Container"
  }

  if (Test-Path (Join-Path $Path ".git") @Container) {
    (Resolve-Path $Path).Path
  }
  else {
    $StrippedPath = ($Path -replace "^\.[\/\\]+", "")

    if ($StrippedPath) {
      $CodeRelativePath = Join-Path $CODE $StrippedPath

      if (Test-Path (Join-Path $CodeRelativePath ".git") @Container) {
        (Resolve-Path $CodeRelativePath).Path
      }
      elseif ($Initialize) {
        $CodeRelativePath
      }
      else {
        $null
      }
    }
    else {
      $null
    }
  }
}

New-Alias gitc Invoke-Repository
New-Alias gg Invoke-Repository
function Invoke-Repository {
  param(
    $Path,
    $Verb,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $DEFAULT_VERB = "status"
  $DEFAULT_PATH = ".\"

  if ($Path) {
    if ($Verb) {
      if (-not ($Verb -in $GIT_VERB)) {
        if ($Path -in $GIT_VERB) {
          if (Resolve-Repository $DEFAULT_PATH) {
            $Option = $Verb
            $Verb = $Path.ToLowerInvariant()
            $Path = $DEFAULT_PATH
          }
          else {
            throw "No 'Path' parameter given, and current directory '$($PWD.Path)' is not a repository."
          }
        }
        else {
          throw "Unknown git verb '$Verb'. Allowed git verbs: $($GIT_VERB -join ', ')."
        }
      }
      elseif (-not (Resolve-Repository $Path)) {
        if (Resolve-Repository $DEFAULT_PATH) {
          $Option = $Path
          $Path = $DEFAULT_PATH
        }
        else {
          throw "Neither 'Path' parameter '$Path' (or '$code\$path') nor current directory '$($PWD.Path)' is a repository."
        }
      }
    }
    else {
      if ($Path -in $GIT_VERB) {
        if (Resolve-Repository $DEFAULT_PATH) {
          $Verb = $Path.ToLowerInvariant()
          $Path = $DEFAULT_PATH
        }
        else {
          throw "No 'Path' parameter given, and current directory '$($PWD.Path)' is not a repository."
        }
      }
      else {
        if (Resolve-Repository $Path) {
          $Verb = $DEFAULT_VERB
        }
        else {
          throw "'Path' parameter '$Path' (or '$code\$path') is not a repository."
        }
      }
    }
  }
  else {
    if (Resolve-Repository $DEFAULT_PATH) {
      $Path = $DEFAULT_PATH
      if (-not $Verb) {
        $Verb = $DEFAULT_VERB
      }
    }
    else {
      throw "No 'Path' parameter given, and current directory '$($PWD.Path)' is not a repository."
    }
  }

  $Repository = Resolve-Repository $Path

  if ($ErrorStop) {
    if ($Local:Option) {
      $Output = git -C $Repository $Verb $Option @args 3>&1 2>&1
    }
    else {
      $Output = git -C $Repository $Verb @args 3>&1 2>&1
    }

    if (($Output -as "string").StartsWith("fatal:")) {
      throw $Output
    }
    else {
      $Output
    }
  }
  else {
    if ($Local:Option) {
      git -C $Repository $Verb $Option @args
    }
    else {
      git -C $Repository $Verb @args
    }
  }
}
