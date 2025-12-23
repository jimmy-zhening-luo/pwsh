using namespace System.Collections.Generic
using namespace Completer
using namespace Completer.PathCompleter

<#
.SYNOPSIS
Test whether a path is the root directory of a Node package.

.DESCRIPTION
This function tests returns true if the supplied path is the root directory of a Node package, otherwise false.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands
#>
function Test-NodePackageDirectory {
  [CmdletBinding()]
  [OutputType([bool])]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [AllowEmptyString()]
    # Node package root path to be resolved
    [string]$WorkingDirectory
  )

  if (-not $WorkingDirectory) {
    $WorkingDirectory = $PWD.Path
  }

  return Test-Path (
    Join-Path $WorkingDirectory package.json
  ) -PathType Leaf
}

<#
.SYNOPSIS
Resolve a Node package at its root directory.

.DESCRIPTION
This function resolves the supplied path to a qualified, rooted path if it is a Node package root. If the supplied path is not a Node package root, an error is thrown.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands
#>
function Resolve-NodePackageDirectory {
  [CmdletBinding()]
  [OutputType([string])]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [AllowEmptyString()]
    # Node package root path to be resolved
    [string]$WorkingDirectory
  )

  if (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory) {
    return $WorkingDirectory ? "--prefix=$((Resolve-Path $WorkingDirectory).Path)" : ''
  }
  else {
    throw "Path '$WorkingDirectory' is not a Node package directory."
  }
}

<#
.SYNOPSIS
Run Node.

.DESCRIPTION
This function is an alias shim for 'node [args]'.

.COMPONENT
Code.Node

.LINK
https://nodejs.org/api/cli.html
#>
function Invoke-Node {

  if ($args) {
    & node.exe @args

    if ($LASTEXITCODE -notin 0, 1) {
      throw "Node.exe error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

[string[]]$NODE_VERB = @(
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

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a command in a Node package.

.DESCRIPTION
This function runs an npm command in a specified Node package directory, or the current directory if no path is specified.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands

.LINK
https://docs.npmjs.com/cli/commands/npm
#>
function Invoke-NodePackage {

  [CmdletBinding()]
  param(

    [Parameter(
      Position = 0
    )]
    [Completions(
      'pkg,i,it,cit,rm,access,adduser,audit,bugs,cache,ci,completion,config,dedupe,deprecate,diff,dist-tag,docs,doctor,edit,exec,explain,explore,find-dupes,fund,help,help-search,init,install,install-ci-test,install-test,link,login,logout,ls,org,outdated,owner,pack,ping,prefix,profile,prune,publish,query,rebuild,repo,restart,root,run,sbom,search,shrinkwrap,star,stars,start,stop,team,test,token,undeprecate,uninstall,unpublish,unstar,update,version,view,whoami'
    )]
    # npm command verb
    [string]$Command,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments
    )]
    # Additional arguments to pass to npm
    [string[]]$Argument,

    # When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    # Show npm version if no command is specified. Otherwise, pass the -v flag.
    [switch]$Version,

    # Pass the -D flag as an argument to npm
    [switch]$D,

    # Pass the -E flag as an argument to npm
    [switch]$E,

    # Pass the -i flag as an argument to npm
    [switch]$I,

    # Pass the -o flag as an argument to npm
    [switch]$O,

    # Pass the -P flag as an argument to npm
    [switch]$P,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add('--color=always')

  $NodeCommand = [List[string]]::new()
  if ($Argument) {
    $NodeCommand.AddRange(
      [List[string]]$Argument
    )
  }

  if ($WorkingDirectory.Length -ne 0) {
    if (
      $WorkingDirectory.StartsWith([char]'-') -or -not (
        Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory
      )
    ) {
      $NodeCommand.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
    else {
      $PackagePrefix = Resolve-NodePackageDirectory -WorkingDirectory $WorkingDirectory

      if ($PackagePrefix) {
        $NodeArgument.Add($PackagePrefix)
      }
    }
  }

  if ($Command.Length -ne 0 -and $Command.StartsWith('-') -or $Command -notin $NODE_VERB -and -not $NODE_ALIAS.ContainsKey($Command)) {
    [string]$DeferredVerb = $NodeCommand.Count -eq 0 ? '' : $NodeCommand.Find(
      {
        $args[0] -in $NODE_VERB
      }
    )

    if ($DeferredVerb) {
      $NodeCommand.Remove($DeferredVerb) | Out-Null
    }

    $NodeCommand.Insert(0, $Command)
    $Command = $DeferredVerb
  }

  if ($Command) {
    $NodeArgument.Add($Command.ToLowerInvariant())
    if ($D) {
      $NodeCommand.Add('-D')
    }
    if ($E) {
      $NodeCommand.Add('-E')
    }
    if ($I) {
      $NodeCommand.Add('-i')
    }
    if ($O) {
      $NodeCommand.Add('-o')
    }
    if ($P) {
      $NodeCommand.Add('-P')
    }
    if ($Version) {
      $NodeCommand.Add('-v')
    }
  }
  else {
    if ($Version) {
      $NodeArgument.Add('-v')
    }
  }

  if ($NodeCommand.Count -ne 0) {
    $NodeArgument.AddRange(
      $NodeCommand
    )
  }

  & npm.ps1 @NodeArgument

  if ($LASTEXITCODE -notin 0, 1) {
    $Exception = "npm command error, execution returned exit code: $LASTEXITCODE"

    if ($NoThrow) {
      Write-Warning -Message "$Exception"
    }
    else {
      throw $Exception
    }
  }
}

<#
.SYNOPSIS
Use 'npx' to run a command from a local or remote npm module.

.DESCRIPTION
This function is an alias shim for 'npx [args]'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npx
#>
function Invoke-NodeExecutable {

  if ($args) {
    & npx.ps1 @args

    if ($LASTEXITCODE -notin 0, 1) {
      throw "npx error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node module cache.

.DESCRIPTION
This function is an alias for 'npm cache clean --force'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {

  $NodeArgument = [List[string]]::new(
    [List[string]]@(
      'clean'
      '--force'
    )
  )
  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-NodePackage -Command cache -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node package.

.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $WorkingDirectory]'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-NodePackage -Command outdated -WorkingDirectory $WorkingDirectory -NoThrow -Argument $NodeArgument
}

enum NodePackageNamedVersion {
  patch
  minor
  major
  prerelease
  preminor
  premajor
}

[regex]$VERSION_SPEC = '^v?(?<Major>(?>\d+))(?>\.(?<Minor>(?>\d*))(?>\.(?<Patch>(?>\d*)))?)?(?>-(?<Pre>(?>\w+)(?>\.(?>\d+))?))?$'

<#
.SYNOPSIS
Use Node Package Manager (npm) to increment the package version of the current Node package.

.DESCRIPTION
This function is an alias for 'npm version [--prefix $WorkingDirectory] [version=patch]'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodePackageVersion {

  param(

    # New package version, default 'patch'
    [Completions(
      'patch,minor,major,prerelease,preminor,premajor'
    )]
    [string]$Version,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  if ($Version) {
    if ($null -eq [NodePackageNamedVersion]::$Version) {
      if ($Version -match $VERSION_SPEC) {
        $FullVersion = @{
          Major = [int]$Matches.Major
          Minor = $Matches.Minor ? [int]$Matches.Minor : 0
          Patch = $Matches.Patch ? [int]$Matches.Patch : 0
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
    else {
      $Version = [NodePackageNamedVersion]::$Version
    }
  }
  else {
    $Version = [NodePackageNamedVersion]::patch
  }

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add(
    $Version.ToLowerInvariant()
  )

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-NodePackage -Command version -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $WorkingDirectory] [--args]'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodePackageScript {

  param(

    # Name of the npm script to run
    [string]$Script,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add($Script)

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-NodePackage -Command run -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run the 'test' script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm test [--prefix $WorkingDirectory] [--args]'.

.COMPONENT
Code.Node

.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-NodePackage -Command test -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

New-Alias no Invoke-Node
New-Alias n Invoke-NodePackage
New-Alias nx Invoke-NodeExecutable
New-Alias ncc Clear-NodeModuleCache
New-Alias npo Compare-NodeModule
New-Alias nu Step-NodePackageVersion
New-Alias nr Invoke-NodePackageScript
New-Alias nt Test-NodePackage
