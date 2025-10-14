function Resolve-Repository {
  param(
    [Parameter(Mandatory)]
    [System.String]$Path,
    [Alias("Clone")]
    [switch]$Initialize
  )
  function Select-ResolvedPath([string] $Path) {
    (Resolve-Path $Path).Path
  }

  $CodeRelativePath = $Path.Contains(":") ? $null : (
    Join-Path $CODE (
      $Path -replace "^\.[\/\\]+", ""
    )
  )
  $Container = @{
    PathType = "Container"
  }

  if ($Initialize) {
    if (Test-Path $Path @Container) {
      Select-ResolvedPath $Path
    }
    elseif (
      $CodeRelativePath -and (
        Test-Path $CodeRelativePath @Container
      )
    ) {
      Select-ResolvedPath $CodeRelativePath
    }
    else {
      $null
    }
  }
  else {
    if (Test-Path (Join-Path $Path ".git") @Container) {
      Select-ResolvedPath $Path
    }
    elseif (
      $CodeRelativePath -and (
        Test-Path (Join-Path $CodeRelativePath ".git") @Container
      )
    ) {
      Select-ResolvedPath $CodeRelativePath
    }
    else {
      $null
    }
  }
}

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
    ? { $_ -like "$wordToComplete*" }
}
Register-ArgumentCompleter -CommandName Invoke-Repository -ParameterName Verb -ScriptBlock $GitVerbArgumentCompleter

New-Alias gitc Invoke-Repository
New-Alias gg Invoke-Repository
function Invoke-Repository {
  param(
    [System.String]$Path,
    [System.String]$Verb,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $DEFAULT_VERB = "status"

  if ($Path) {
    if ($Verb) {
      if (-not ($Verb -in $GIT_VERB)) {
        if ($Path -in $GIT_VERB) {
          if (Resolve-Repository $PWD.Path) {
            $Option = $Verb
            $Verb = $Path.ToLowerInvariant()
            $Path = $PWD.Path
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
        if (Resolve-Repository $PWD.Path) {
          $Option = $Path
          $Path = $PWD.Path
        }
        else {
          throw "Neither 'Path' parameter '$Path' (or '$code\$path') nor current directory '$($PWD.Path)' is a repository."
        }
      }
    }
    else {
      if ($Path -in $GIT_VERB) {
        if (Resolve-Repository $PWD.Path) {
          $Verb = $Path.ToLowerInvariant()
          $Path = $PWD.Path
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
    if (Resolve-Repository $PWD.Path) {
      $Path = $PWD.Path

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
