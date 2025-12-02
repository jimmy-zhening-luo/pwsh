$GIT_VERB = (
  Import-PowerShellDataFile (
    Join-Path $PSScriptRoot 'Git-Verb.psd1' -Resolve
  ) -ErrorAction Stop
).GIT_VERB

New-Alias gg Git\Invoke-Repository
function Invoke-Repository {
  param(
    [string]$Path,
    [string]$Verb,
    [switch]$Throw
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
      throw "'git status' requires an existing repository. The current directory is not a repository, and no other path was provided."
    }
  }
  elseif (-not $Path) {
    if ($Verb -in $GIT_VERB) {
      $Verb = $Verb.ToLowerInvariant()

      $Resolve = @{
        Path = $Path
        New  = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. The current directory is not a repository, and no other path was provided."
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
        Path = $Path
        New  = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. The current directory is not a repository, and no other path was provided."
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
        Path = $Path
        New  = $Verb -eq 'clone'
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
        Path = $Path
        New  = $Verb -eq 'clone'
      }

      if (Resolve-Repository @Resolve) {
        $GitArguments += (Resolve-Repository @Resolve), $Verb
      }
      else {
        throw "'git $Verb' requires an existing repository. The current directory is not a repository, and no other path was provided."
      }
    }
    else {
      throw "Unknown git verb '$Verb' or '$Path'. Allowed git verbs: $($GIT_VERB -join ', ')."
    }
  }

  $GitArguments += $GitOptions
  $GitArguments = '-c', 'color.ui=always' + $GitArguments

  if ($Throw) {
    $GitOutput = ''

    & git $GitArguments @args 2>&1 |
      Tee-Object -Variable GitOutput

    if (($GitOutput -as [string]).StartsWith('fatal:')) {
      throw $GitOutput
    }
  }
  else {
    & git $GitArguments @args
  }
}

function Resolve-Repository {
  param(
    [string]$Path,
    [switch]$New
  )

  $CODE = Join-Path $HOME 'code'
  $Item = @{
    Path = $Path
  }
  $Repository = ''

  if ($New) {
    if (Shell\Test-Item @Item) {
      $Repository = Resolve-Item @Item
    }
    else {
      $Item.Location = $CODE
      $Item.New = $True

      if (Shell\Test-Item @Item) {
        $Repository = Resolve-Item @Item
      }
    }
  }
  else {
    $GitPath = $Path ? (Join-Path $Path '.git') : '.git'
    $Git = @{
      Path           = $GitPath
      RequireSubpath = $True
    }

    if (Shell\Test-Item @Git) {
      $Repository = Resolve-Item @Item
    }
    else {
      $Item.Location = $CODE
      $Git.Location = $CODE

      if (Shell\Test-Item @Git) {
        $Repository = Resolve-Item @Item
      }
    }
  }

  $Repository
}
