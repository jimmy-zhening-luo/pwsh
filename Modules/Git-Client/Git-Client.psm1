$GIT_VERB =
Import-PowerShellDataFile (
  Join-Path $PSScriptRoot "Git-Verb.psd1" -Resolve
) -ErrorAction Stop |
  Select-Object -ExpandProperty GIT_VERB
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
    [string]$Path,
    [string]$Verb,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $GitArguments = , '-C'
  $GitOptions = @()

  if ($Path.StartsWith('-')) {
    $GitOptions += $Path
    $Path = ''

    if ($Verb) {
      if ($Verb.StartsWith('-')) {
        $GitOptions += $Verb
        $Verb = ''
      }
      else {
        $Path = $Verb
        $Verb = ''
      }
    }
  }

  if ($Verb.StartsWith('-')) {
    $GitOptions += $Verb
    $Verb = ''
  }

  if (-not $Path -and -not $Verb) {
    if (Resolve-Repository $Path) {
      $GitArguments += (Resolve-Repository $Path), 'status'
    }
    else {
      throw "'git status' requires an existing repository. The current directory '$($PWD.Path)' is not a repository, and no other path was provided."
    }
  }
  elseif (-not $Path) {
    if ($Verb -in $GIT_VERB) {
      $Verb = $Verb.ToLowerInvariant()

      $Resolve = @{
        Path       = $Path
        Initialize = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. '$($PWD.Path)' is not a repository, and no other path was provided."
      }
    }
    else {
      throw "Unknown git verb '$Verb'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }
  }
  elseif (-not $Verb) {
    if (Resolve-Repository -Path $Path) {
      $GitArguments += (Resolve-Repository -Path $Path), 'status'
    }
    elseif ($Path -in $GIT_VERB) {
      $Verb = $Path.ToLowerInvariant()
      $Path = ''

      $Resolve = @{
        Path       = $Path
        Initialize = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. '$($PWD.Path)' is not a repository, and no other path was provided."
      }
    }
    else {
      throw "'git status' requires an existing repository. Neither '$Path' nor '$code\$path' is a repository."
    }
  }
  else {
    if ($Verb -in $GIT_VERB) {
      $Verb = $Verb.ToLowerInvariant()

      $Resolve = @{
        Path       = $Path
        Initialize = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "Path '$Path' is not a valid target for 'git $Verb'."
      }
    }
    elseif ($Path -in $GIT_VERB) {
      $Verb = $Path.ToLowerInvariant()
      $Path = ''

      $Resolve = @{
        Path       = $Path
        Initialize = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. '$($PWD.Path)' is not a repository, and no other path was provided."
      }
    }
    else {
      throw "Unknown git verb '$Verb' or '$Path'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }
  }

  $GitArguments += $GitOptions

  if ($ErrorStop) {
    $GitOutput = git $GitArguments @args 2>&1

    if (($GitOutput -as [string]).StartsWith("fatal:")) {
      throw $GitOutput
    }
    else {
      $GitOutput
    }
  }
  else {
    git $GitArguments @args
  }
}

function Resolve-Repository {
  [OutputType([string])]
  param(
    [string]$Path,
    [Alias("Clone")]
    [switch]$Initialize
  )

  function Get-CodeRelativePath([string]$Path) {
    Join-Path ~\code ($Path -replace "^\.[\/\\]+", '')
  }

  function Add-Git([string]$Path) {
    Join-Path $Path '.git'
  }

  function Select-ResolvedPath([string]$Path) {
    Resolve-Path -Path $Path |
      Select-Object -ExpandProperty Path
  }

  $Container = @{
    PathType = 'Container'
  }

  if ($Initialize) {
    if ($Path) {
      if (Test-Path -Path $Path @Container) {
        Select-ResolvedPath $Path
      }
      elseif (
        -not $Path.contains(':') -and (
          Test-Path -Path (Get-CodeRelativePath $Path) @Container
        )
      ) {
        Select-ResolvedPath $CodeRelativePath
      }
      else {
        ''
      }
    }
    else {
      Select-ResolvedPath ~\code
    }
  }
  else {
    if ($Path) {
      if (Test-Path -Path (Add-Git $Path) @Container) {
        Select-ResolvedPath $Path
      }
      elseif (
        -not $Path.contains(':') -and (
          Test-Path -Path (
            Add-Git (Get-CodeRelativePath $Path)
          ) @Container
        )
      ) {
        Select-ResolvedPath (Get-CodeRelativePath $Path)
      }
      else {
        ''
      }
    }
    elseif (Test-Path -Path (Add-Git $PWD.Path) @Container) {
      Select-ResolvedPath $PWD.Path
    }
    else {
      ''
    }
  }
}
