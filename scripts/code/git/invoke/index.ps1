. $PSScriptRoot\resolve.ps1

New-Alias gitc Invoke-Repository
function Invoke-Repository {
  param(
    [string]$Path,
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
    [string]$Verb
  )

  $VERB_LIST = @(
    "add",
    "clone",
    "commit",
    "pull",
    "push",
    "reset",
    "status",
    "switch"
  )
  $DEFAULT_VERB = "status"
  $DEFAULT_PATH = ".\"
  $Option = $null

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
          # If $Path and $Verb are both provided, but neither is a verb, the request is definitely malformed: make no further attempts to resolve.

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
      # If $Path is supplied BUT $Verb is not supplied, very weird and very short leash.

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

  if ($Option) {
    git -C $Repository $Verb $Option @args
  }
  else {
    git -C $Repository $Verb @args
  }
}
