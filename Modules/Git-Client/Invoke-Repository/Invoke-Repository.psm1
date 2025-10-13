New-Alias gitc Invoke-Repository
New-Alias gg Invoke-Repository
function Invoke-Repository {
  param(
    [System.String]$Path,
    [ValidateSet([GitVerb])]
    [System.String]$Verb
  )

  $GIT_VERB = (Import-PowerShellDataFile (Join-Path $PSScriptRoot "Git-Verb.psd1" -Resolve) -ErrorAction Stop).GIT_VERB
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

  if ($Local:Option) {
    $Output = git -C $Repository $Verb $Option @args 2>&1
  }
  else {
    $Output = git -C $Repository $Verb @args 2>&1
  }

  if (($Output -as "string").StartsWith("fatal:")) {
    throw $Output
  }
  else {
    $Output
  }
}

function Resolve-Repository {
  param([System.String]$Path)

  if (Test-Path (Join-Path $Path ".git") -PathType Container) {
    Resolve-Path $Path
  }
  else {
    $CodeSubpath = Join-Path $CODE ($Path -replace "^\.[\/\\]+", "")

    if (Test-Path (Join-Path $CodeSubpath ".git") -PathType Container) {
      Resolve-Path $CodeSubpath
    }
    else {
      $null
    }
  }
}
