using namespace System.Collections.Generic
using namespace Completer
using namespace Completer.PathCompleter

<#
.SYNOPSIS
Test whether a path is the root directory of a Node package.

.DESCRIPTION
This function tests returns true if the supplied path is the root directory of a Node package, otherwise false.

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

  [hashtable]$Private:HasPackageJson = @{
    Path     = Join-Path $WorkingDirectory package.json
    PathType = 'Leaf'
  }
  return Test-Path @HasPackageJson
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

  [hashtable]$Private:TestWorkingDirectory = @{
    WorkingDirectory = $WorkingDirectory
  }
  if (Test-NodePackageDirectory @TestWorkingDirectory) {
    if ($WorkingDirectory) {
      [hashtable]$Private:ResolveWorkingDirectory = @{
        Path = $WorkingDirectory
      }
      [string]$Private:WorkingDirectoryPath = (Resolve-Path @ResolveWorkingDirectory).Path

      return "--prefix=$WorkingDirectoryPath"
    }
    else {
      return ''
    }
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

.LINK
https://nodejs.org/api/cli.html
#>
function Invoke-Node {

  if ($args) {
    & node.exe @args

    if ($LASTEXITCODE -ne 0) {
      throw "Node.exe error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

$NODE_VERB = @(
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
    [StaticCompletions(
      'pkg,i,it,cit,rm,access,adduser,audit,bugs,cache,ci,completion,config,dedupe,deprecate,diff,dist-tag,docs,doctor,edit,exec,explain,explore,find-dupes,fund,help,help-search,init,install,install-ci-test,install-test,link,login,logout,ls,org,outdated,owner,pack,ping,prefix,profile,prune,publish,query,rebuild,repo,restart,root,run,sbom,search,shrinkwrap,star,stars,start,stop,team,test,token,undeprecate,uninstall,unpublish,unstar,update,version,view,whoami',
      $null, $null
    )]
    # npm command verb
    [string]$Command,

    [PathLocationCompletions(
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
    [string[]]$NodeArgument,

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

    [Parameter(DontShow)][switch]$zNothing

  )

  $Private:NodeArgumentList = [List[string]]::new(
    [List[string]]@(
      '--color=always'
    )
  )

  $Private:CallerNodeArgument = [List[string]]::new()
  if ($NodeArgument) {
    $CallerNodeArgument.AddRange(
      [List[string]]$NodeArgument
    )
  }

  if ($WorkingDirectory.Length -ne 0) {
    if ($WorkingDirectory.StartsWith('-') -or -not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $CallerNodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
    else {
      [string]$Private:PackagePrefix = Resolve-NodePackageDirectory -WorkingDirectory $WorkingDirectory

      if ($PackagePrefix) {
        $NodeArgumentList.Add($PackagePrefix)
      }
    }
  }

  if ($Command.Length -ne 0 -and $Command.StartsWith('-') -or $Command -notin $NODE_VERB -and -not $NODE_ALIAS.ContainsKey($Command)) {
    [string]$Private:DeferredVerb = $CallerNodeArgument.Count -eq 0 ? '' : $CallerNodeArgument.Find(
      {
        $args[0] -in $NODE_VERB
      }
    )

    if ($DeferredVerb) {
      $CallerNodeArgument.Remove($DeferredVerb) | Out-Null
    }

    $CallerNodeArgument.Insert(0, $Command)
    $Command = $DeferredVerb
  }

  if ($Command) {
    $NodeArgumentList.Add($Command.ToLowerInvariant())

    if ($D) {
      $CallerNodeArgument.Add('-D')
    }
    if ($E) {
      $CallerNodeArgument.Add('-E')
    }
    if ($I) {
      $CallerNodeArgument.Add('-i')
    }
    if ($O) {
      $CallerNodeArgument.Add('-o')
    }
    if ($P) {
      $CallerNodeArgument.Add('-P')
    }
    if ($Version) {
      $CallerNodeArgument.Add('-v')
    }
  }
  else {
    if ($Version) {
      $NodeArgumentList.Add('-v')
    }
  }

  if ($CallerNodeArgument.Count -ne 0) {
    $NodeArgumentList.AddRange(
      $CallerNodeArgument
    )
  }

  & npm.ps1 @NodeArgumentList

  if ($LASTEXITCODE -ne 0) {
    [string]$Private:Exception = "npm command error, execution returned exit code: $LASTEXITCODE"

    if ($NoThrow) {
      Write-Warning -Message $Exception
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

.LINK
https://docs.npmjs.com/cli/commands/npx
#>
function Invoke-NodeExecutable {

  if ($args) {
    & npx.ps1 @args

    if ($LASTEXITCODE -ne 0) {
      throw "npx error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node module cache.

.DESCRIPTION
This function is an alias for 'npm cache clean --force'.

.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {

  $Private:NodeArgument = [List[string]]::new(
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

  [hashtable]$Private:CacheClean = @{
    Command      = 'cache'
    NodeArgument = $NodeArgument
  }
  Invoke-NodePackage @CacheClean
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node package.

.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $WorkingDirectory]'.

.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$zNothing

  )

  $Private:NodeArgument = [List[string]]::new()

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

  [hashtable]$Private:Outdated = @{
    Command          = 'outdated'
    WorkingDirectory = $WorkingDirectory
    NodeArgument     = $NodeArgument
    NoThrow          = $True
  }
  Invoke-NodePackage @Outdated
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

.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodePackageVersion {

  param(

    # New package version, default 'patch'
    [StaticCompletions(
      'patch,minor,major,prerelease,preminor,premajor',
      $null, $null
    )]
    [string]$Version,

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$zNothing

  )

  if ($Version) {
    if ($null -eq [NodePackageNamedVersion]::$Version) {
      if ($Version -match $VERSION_SPEC) {
        [hashtable]$Private:FullVersion = @{
          Major = [uint]$Matches.Major
          Minor = $Matches.Minor ? [uint]$Matches.Minor : [uint]0
          Patch = $Matches.Patch ? [uint]$Matches.Patch : [uint]0
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

  $Private:NodeArgument = [List[string]]::new(
    [List[string]]@(
      $Version.ToLowerInvariant()
    )
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

  [hashtable]$Private:StepVersion = @{
    Command          = 'version'
    WorkingDirectory = $WorkingDirectory
    NodeArgument     = $NodeArgument
  }
  Invoke-NodePackage @StepVersion
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $WorkingDirectory] [--args]'.

.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodePackageScript {

  param(

    # Name of the npm script to run
    [string]$Script,

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$zNothing

  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $Private:NodeArgument = [List[string]]::new(
    [List[string]]@(
      $Script
    )
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

  [hashtable]$Private:RunScript = @{
    Command          = 'run'
    WorkingDirectory = $WorkingDirectory
    NodeArgument     = $NodeArgument
  }
  Invoke-NodePackage @RunScript
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run the 'test' script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm test [--prefix $WorkingDirectory] [--args]'.

.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$zNothing

  )

  $Private:NodeArgument = [List[string]]::new()

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

  [hashtable]$Private:Test = @{
    Command          = 'test'
    WorkingDirectory = $WorkingDirectory
    NodeArgument     = $NodeArgument
  }
  Invoke-NodePackage @Test
}

New-Alias no Invoke-Node
New-Alias n Invoke-NodePackage
New-Alias nx Invoke-NodeExecutable
New-Alias ncc Clear-NodeModuleCache
New-Alias npo Compare-NodeModule
New-Alias nu Step-NodePackageVersion
New-Alias nr Invoke-NodePackageScript
New-Alias nt Test-NodePackage
