New-Alias gitc Invoke-Repository
New-Alias gg Invoke-Repository
function Invoke-Repository {
  param(
    [System.String]$Path,
    [ArgumentCompletions(
      "add",
      "clone",
      "commit",
      "pull",
      "push",
      "reset",
      "status",
      "switch"
    )]
    [System.String]$Verb
  )

  $VERB_LIST = @(
    "add"
    "clone"
    "commit"
    "pull"
    "push"
    "reset"
    "status"
    "switch"
  )
  $DEFAULT_VERB = "status"
  $DEFAULT_PATH = ".\"

  if ($Path) {
    if ($Verb) {
      if (-not ($Verb -in $VERB_LIST)) {
        if ($Path -in $VERB_LIST) {
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
          throw "Unknown git verb '$Verb'. Allowed git verbs: $($VERB_LIST -join ', ')."
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
      if ($Path -in $VERB_LIST) {
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
    git -C $Repository $Verb $Option @args
  }
  else {
    git -C $Repository $Verb @args
  }
}
