New-Alias gitcl Git\Import-Repository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-Repository {
  param(
    [string]$Repository,
    [string]$Path,
    [switch]$Throw,
    [switch]$ForceSsh
  )

  $Local:args = $args

  if ($Path.StartsWith('-')) {
    $Local:args = , $Path + $Local:args
    $Path = ''
  }

  $RepositoryPathParts = $Repository -split '/' -notmatch '^\s*$'

  if (-not $RepositoryPathParts) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathParts.Count -eq 1) {
    $RepositoryPathParts = , 'jimmy-zhening-luo' + $RepositoryPathParts
  }

  $Protocol = $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  $Origin = $Protocol + ($RepositoryPathParts -join '/')
  $Local:args = , $Origin + $Local:args
  $Clone = @{
    Path  = $Path
    Verb  = 'clone'
    Throw = $Throw
  }
  Invoke-Repository @Clone @Local:args
}

New-Alias gitp Git\Get-Repository
New-Alias ggp Git\Get-Repository
<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for 'git pull'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-Repository {
  param(
    [string]$Path,
    [switch]$Throw
  )

  $Pull = @{
    Verb = 'pull'
  }
  Invoke-Repository @Pull @PSBoundParameters @args
}

New-Alias gpa Git\Get-ChildRepository
<#
.SYNOPSIS
Use Git to pull changes from all child repositories.
.DESCRIPTION
This function retrieves all child repositories in %USERPROFILE%\code\' and pulls changes from each one.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildRepository {
  $Code = @{
    Path      = "$HOME\code"
    Directory = $True
  }
  $Repositories = Get-ChildItem @Code |
    Select-Object -ExpandProperty FullName |
    Resolve-Repository
  $Count = $Repositories.Count

  foreach ($Repository in $Repositories) {
    Get-Repository -Path $Repository @args
  }

  "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

New-Alias gga Git\Add-Repository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for 'git add .' and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-Repository {
  param(
    [string]$Path,
    [switch]$Throw,
    [Alias('r')]
    [switch]$Renormalize
  )

  $Local:args = $args
  $All = '.'
  $fRenormalize = '--renormalize'

  if ($All -notin $args) {
    $Local:args = , $All + $Local:args
  }

  if ($Renormalize -and $fRenormalize -notin $args) {
    $Local:args += $fRenormalize
  }

  $Add = @{
    Path  = $Path
    Verb  = 'add'
    Throw = $Throw
  }
  Invoke-Repository @Add @Local:args
}

New-Alias ggm Git\Write-Repository
<#
.SYNOPSIS
Commit changes to a Git repository.
.DESCRIPTION
This function commits changes to a Git repository using the 'git commit' command.
.LINK
https://git-scm.com/docs/git-commit
#>
function Write-Repository {
  param(
    [string]$Path,
    [string]$Message,
    [switch]$Throw,
    [switch]$AllowEmpty
  )

  $Local:args = $args

  if ($Message) {
    $Local:args = , $Message + $Local:args
  }

  $CommitArguments, $Messages = $Local:args.Where(
    { $_ -and $_ -is [string] }
  ).Where(
    { $_ -match '^-(?>\w|-\w+)$' },
    'Split'
  )

  if ($Path) {
    if (-not ($Path | Resolve-Repository)) {
      if ($Path -match '^-(?>\w|-\w+)$') {
        $CommitArguments = , $Path + $CommitArguments
      }
      else {
        $Messages = , $Path + $Messages
      }

      $Path = ''
    }
  }

  $fAllowEmpty = '--allow-empty'

  if ($AllowEmpty) {
    if ($fAllowEmpty -notin $CommitArguments) {
      $CommitArguments += $fAllowEmpty
    }
  }
  else {
    if ($fAllowEmpty -in $CommitArguments) {
      $AllowEmpty = $True
    }
  }

  if (-not $Messages) {
    if ($AllowEmpty) {
      $Messages += 'No message.'
    }
    else {
      throw 'Missing commit message.'
    }
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-Repository @Parameters -Throw

  $CommitArguments = '-m', ($Messages -join ' ') + $CommitArguments
  $Commit = @{
    Verb = 'commit'
  }
  Invoke-Repository @Commit @Parameters @CommitArguments
}

New-Alias ggs Git\Push-Repository
<#
.SYNOPSIS
Use Git to push changes to a repository.
.DESCRIPTION
This function is an alias for 'git push'.
.LINK
https://git-scm.com/docs/git-push
#>
function Push-Repository {
  param(
    [string]$Path,
    [switch]$Throw
  )

  Get-Repository @PSBoundParameters -Throw

  $Push = @{
    Verb = 'push'
  }
  Invoke-Repository @Push @PSBoundParameters @args
}

New-Alias gr Git\Reset-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [[[HEAD]~][n=1]]'.
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-Repository {
  param(
    [string]$Path,
    [string]$Tree,
    [switch]$Throw
  )

  if ($Path -eq '~' -and -not $Tree -or -not ($Path | Resolve-Repository)) {
    $Path, $Tree = '', $Path
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-Repository @Parameters -Throw

  $Local:args = $args

  if ($Tree) {
    if ($Tree -match '^(?>head)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))' -and (-not $Matches.Step -or $Matches.Step -as [uint32])) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }

    $Local:args = , $Tree + $Local:args
  }

  $Local:args = , '--hard' + $Local:args
  $Reset = @{
    Verb = 'reset'
  }
  Invoke-Repository @Reset @Parameters @Local:args
}

New-Alias grp Git\Restore-Repository
<#
.SYNOPSIS
Use Git to restore a repository to its previous state.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.
.LINK
https://git-scm.com/docs/git-reset
.LINK
https://git-scm.com/docs/git-pull
#>
function Restore-Repository {
  param(
    [string]$Path,
    [switch]$Throw
  )

  Reset-Repository @PSBoundParameters -Throw @args
  Get-Repository @PSBoundParameters
}
