using namespace System.Collections.Generic
using namespace System.Management.Automation

<#
.SYNOPSIS
Test whether a path is the root directory of a Node package.
.DESCRIPTION
This function tests returns true if the supplied path is the root directory of a Node package, otherwise false.
.LINK
https://docs.npmjs.com/cli/commands
#>
function Test-NodePackageDirectory {
  [OutputType([string])]
  param(
    [PathCompletions('.', 'Directory')]
    # Node package root path to be resolved
    [string]$Path
  )

  $Private:IsNodePackage = @{
    Path     = Join-Path ($Path ? $Path : $PWD) package.json
    PathType = 'Leaf'
  }
  return Test-Path @IsNodePackage
}

<#
.SYNOPSIS
Resolve a Node package at its root directory.
.DESCRIPTION
This function resolves the supplied path to a qualified, rooted path if it is a Node package root. If the supplied path is not a Node package root, an error is thrown.
.LINK
https://docs.npmjs.com/cli/commands
#>
function Resolve-NodePackageDirectory {
  [OutputType([string])]
  param(
    [PathCompletions('.', 'Directory')]
    # Node package root path to be resolved
    [string]$Path,
    # Omit the '--prefix=' prefix from the output
    [switch]$OmitPrefix
  )

  $Private:IsNodePackage = @{
    Path = $Path
  }
  if (Test-NodePackageDirectory @IsNodePackage) {
    $Package = ($Path ? (Resolve-Path $Path) : $PWD).Path

    $OmitPrefix ? $Package : "--prefix=$Package"
  }
  else {
    throw "Path '$Path' is not a Node package directory."
  }
}

New-Alias no Invoke-Node
<#
.SYNOPSIS
Run Node.
.DESCRIPTION
This function is an alias shim for 'node [args]'.
.LINK
https://nodejs.org/api/cli.html
#>
function Invoke-Node {
  & node.exe @args
}

$NODE_VERB = @(
  # Name
  'access'
  'adduser'
  'audit'
  'bugs'
  'cache'
  'ci'
  'completion'
  'config'
  'dedupe'
  'deprecate'
  'diff'
  'dist-tag'
  'docs'
  'doctor'
  'edit'
  'exec'
  'explain'
  'explore'
  'find-dupes'
  'fund'
  'help'
  'help-search'
  'init'
  'install'
  'install-ci-test'
  'install-test'
  'link'
  'login'
  'logout'
  'ls'
  'org'
  'outdated'
  'owner'
  'pack'
  'ping'
  'pkg'
  'prefix'
  'profile'
  'prune'
  'publish'
  'query'
  'rebuild'
  'repo'
  'restart'
  'root'
  'run'
  'sbom'
  'search'
  'shrinkwrap'
  'star'
  'stars'
  'start'
  'stop'
  'team'
  'test'
  'token'
  'undeprecate'
  'uninstall'
  'unpublish'
  'unstar'
  'update'
  'version'
  'view'
  'whoami'
)

$NODE_ALIAS = @{
  issues  = 'bugs'
  c       = 'config'
  ddp     = 'dedupe'
  home    = 'docs'
  why     = 'explain'
  create  = 'init'
  add     = 'install'
  i       = 'install'
  in      = 'install'
  ln      = 'link'
  cit     = 'install-ci-test'
  it      = 'install-test'
  list    = 'ls'
  author  = 'owner'
  rb      = 'rebuild'
  find    = 'search'
  s       = 'search'
  se      = 'search'
  t       = 'test'
  unlink  = 'uninstall'
  remove  = 'uninstall'
  rm      = 'uninstall'
  r       = 'uninstall'
  un      = 'uninstall'
  up      = 'update'
  upgrade = 'update'
  info    = 'view'
  show    = 'view'
  v       = 'view'
}

New-Alias n Invoke-NodePackage
<#
.SYNOPSIS
Use Node Package Manager (npm) to run a command in a Node package.
.DESCRIPTION
This function is an alias shim for 'npm [args]'.
.LINK
https://docs.npmjs.com/cli/commands
.LINK
https://docs.npmjs.com/cli/commands/npm
#>
function Invoke-NodePackage {
  param(
    [Parameter(
      Position = 0
    )]
    [GenericCompletions(
      'access,adduser,audit,bugs,cache,ci,completion,config,dedupe,deprecate,diff,dist-tag,docs,doctor,edit,exec,explain,explore,find-dupes,fund,help,help-search,init,install,install-ci-test,install-test,link,login,logout,ls,org,outdated,owner,pack,ping,pkg,prefix,profile,prune,publish,query,rebuild,repo,restart,root,run,sbom,search,shrinkwrap,star,stars,start,stop,team,test,token,undeprecate,uninstall,unpublish,unstar,update,version,view,whoami'
    )]
    [string]$Verb,
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root
    [string]$Path
  )

  if (-not $Verb) {
    throw 'No npm verb specified.'
  }

  # npm --color [--prefix] <command> [command options] [arguments...]
  $Private:NodeArguments = [List[string]]::new()
  $NodeArguments.Add('--color=always')

  $Private:Package = $Path ? (Resolve-NodePackageDirectory -Path $Path) : ''
  if ($Package) {
    $NodeArguments.Add($Package)
  }

  $Private:NodeCommandArguments = [List[string]]::new()
  if ($args) {
    $NodeCommandArguments.AddRange([List[string]]$args)
  }

  if ($Verb.StartsWith('-') -or $Verb -notin $NODE_VERB -and -not $NODE_ALIAS.ContainsKey($Verb)) {
    $Private:DeferredVerb = $NodeCommandArguments.Find(
      {
        $args[0] -in $NODE_VERB
      }
    )

    "DeferredVerb: $DeferredVerb"

    if (-not $DeferredVerb) {
      throw "Unrecognized npm verb '$Verb'."
    }

    $NodeCommandArguments.Remove($DeferredVerb) | Out-Null
    $Verb = $DeferredVerb
  }

  $Verb = $Verb.ToLowerInvariant()

  $NodeArguments.Add($Verb)

  if ($NodeCommandArguments.Count -ne 0) {
    $NodeArguments.AddRange($NodeCommandArguments)
  }

  & npm.ps1 @NodeArguments 2>&1 |
    Tee-Object -Variable NpmResult

  if ($NpmResult) {
    $Private:NpmError = @()

    if ($NpmResult -is [array]) {
      $Private:ErrorRecords = $NpmResult |
        Where-Object {
          $_ -is [ErrorRecord]
        }

      if ($ErrorRecords) {
        $NpmError += $ErrorRecords
      }
      else {
        $Private:Strings = $NpmResult |
          Where-Object { $_ -is [string] } |
          Where-Object { $_ -match '^npm error' }

        if ($Strings) {
          $NpmError += $Strings
        }
      }
    }
    elseif ($NpmResult -is [ErrorRecord] -or $NpmResult -is [string] -and $NpmResult -match '^npm error') {
      $NpmError += $NpmResult
    }

    if ($NpmError) {
      throw 'Npm command error, execution stopped.'
    }
  }
}

New-Alias nx Invoke-NodeExecutable
<#
.SYNOPSIS
Use 'npx' to run a command from a local or remote npm module.
.DESCRIPTION
This function is an alias shim for 'npx [args]'.
.LINK
https://docs.npmjs.com/cli/commands/npx
#>
function Invoke-NodeExecutable {
  & npx.ps1 @args
}

New-Alias ncc Clear-NodeModuleCache
<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node module cache.
.DESCRIPTION
This function is an alias for 'npm cache clean --force'.
.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {
  & npm.ps1 cache clean --force @args
}

New-Alias npo Compare-NodeModule
<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node package.
.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $Path]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {
  param(
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root
    [string]$Path
  )

  $NodeArguments = $args
  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $PSBoundParameters.Path = ''
  }
  $Package = Resolve-NodePackageDirectory @PSBoundParameters

  if ($Package) {
    $NodeArguments = , $Package + $NodeArguments
  }

  & npm.ps1 outdated @NodeArguments
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to increment the package version of the current Node package.
.DESCRIPTION
This function is an alias for 'npm version [--prefix $Path] [version=patch]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodePackageVersion {
  param(
    # New package version, default 'patch'
    [GenericCompletions('patch,minor,major,prerelease,preminor,premajor')]
    [string]$Version,
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root
    [string]$Path
  )

  $NodeArguments = $args
  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $PSBoundParameters.Path = ''
  }
  $Package = Resolve-NodePackageDirectory @PSBoundParameters

  if ($Package) {
    $NodeArguments = , $Package + $NodeArguments
  }

  $NAMED_VERSION = @(
    'patch'
    'minor'
    'major'
    'prerelease'
    'preminor'
    'premajor'
  )
  if ($Version) {
    if ($Version -notin $NAMED_VERSION) {
      if ($Version -match '^v?(?<Major>(?>\d+))(?>\.(?<Minor>(?>\d*))(?>\.(?<Patch>\(?>d*)))?)?(?>-(?<Pre>(?>\w+)(?>\.(?>\d+))?))?$') {
        $FullVersion = @{
          Major = [UInt32]$Matches.Major
          Minor = $Matches.Minor ? [UInt32]$Matches.Minor : [UInt32]0
          Patch = $Matches.Patch ? [UInt32]$Matches.Patch : [UInt32]0
          Pre   = $Matches.Pre ? [string]$Matches.Pre : ''
        }

        $Version = "$($FullVersion.Major).$($FullVersion.Minor).$($FullVersion.Patch)"

        if ($FullVersion.Pre) {
          $Version += "-$($FullVersion.Pre)"
        }
      }
      else {
        throw "Unrecognized version ''"
      }
    }
  }
  else {
    $Version = 'patch'
  }

  $Version = $Version.ToLowerInvariant()

  & npm.ps1 version $Version @NodeArguments
}

New-Alias nr Invoke-NodePackageScript
<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node package's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $Path] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodePackageScript {
  param(
    # Name of the npm script to run
    [string]$Script,
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root
    [string]$Path
  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $NodeArguments = $args
  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $PSBoundParameters.Path = ''
  }
  $Package = Resolve-NodePackageDirectory @PSBoundParameters

  if ($Package) {
    $NodeArguments = , $Package + $NodeArguments
  }

  & npm.ps1 run $Script @NodeArguments
}

New-Alias nt Test-NodePackage
<#
.SYNOPSIS
Use Node Package Manager (npm) to run the 'test' script defined in a Node package's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm test [--prefix $Path] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {
  param(
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root
    [string]$Path
  )

  $NodeArguments = $args
  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $PSBoundParameters.Path = ''
  }
  $Package = Resolve-NodePackageDirectory @PSBoundParameters

  if ($Package) {
    $NodeArguments = , $Package + $NodeArguments
  }

  & npm.ps1 test @NodeArguments
}
