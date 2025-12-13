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
    $WorkingDirectory = $PWD
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
      [string]$Private:WorkingDirectoryPath = Resolve-Path @ResolveWorkingDirectory

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

  & node.exe @args

  if ($LASTEXITCODE -ne 0) {
    throw "Node.exe error, execution stopped with exit code: $LASTEXITCODE"
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

  param(

    [Parameter(
      Position = 0
    )]
    [GenericCompletions(
      {
        return @(
          'pkg'
          'i'
          'it'
          'cit'
          'rm'
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
      }
    )]
    # npm command verb
    [string]$Command,

    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments
    )]
    # Additional arguments to pass to npm
    [string[]]$NodeArguments,

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
    [switch]$P

  )

  $Private:NodeArgumentList = [List[string]]::new(
    [List[string]]@(
      '--color=always'
    )
  )

  $Private:CallerNodeArguments = [List[string]]::new()
  if ($NodeArguments) {
    $CallerNodeArguments.AddRange(
      [List[string]]$NodeArguments
    )
  }

  if ($WorkingDirectory.Length -ne 0) {
    if ($WorkingDirectory.StartsWith('-') -or -not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $CallerNodeArguments.Add($WorkingDirectory)
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
    [string]$Private:DeferredVerb = $CallerNodeArguments.Count -eq 0 ? '' : $CallerNodeArguments.Find(
      {
        $args[0] -in $NODE_VERB
      }
    )

    if ($DeferredVerb) {
      $CallerNodeArguments.Remove($DeferredVerb) | Out-Null
    }

    $CallerNodeArguments.Insert(0, $Command)
    $Command = $DeferredVerb
  }

  if ($Command) {
    $NodeArgumentList.Add($Command.ToLowerInvariant())

    if ($D) {
      $CallerNodeArguments.Add('-D')
    }
    if ($E) {
      $CallerNodeArguments.Add('-E')
    }
    if ($I) {
      $CallerNodeArguments.Add('-i')
    }
    if ($O) {
      $CallerNodeArguments.Add('-o')
    }
    if ($P) {
      $CallerNodeArguments.Add('-P')
    }
    if ($Version) {
      $CallerNodeArguments.Add('-v')
    }
  }
  else {
    if ($Version) {
      $NodeArgumentList.Add('-v')
    }
  }

  if ($CallerNodeArguments.Count -ne 0) {
    $NodeArgumentList.AddRange(
      $CallerNodeArguments
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

  & npx.ps1 @args

  if ($LASTEXITCODE -ne 0) {
    throw "npx error, execution stopped with exit code: $LASTEXITCODE"
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

  $Private:NodeArguments = [List[string]]::new(
    [List[string]]@(
      'clean'
      '--force'
    )
  )
  if ($args) {
    $NodeArguments.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:CacheClean = @{
    Command       = 'cache'
    NodeArguments = $NodeArguments
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

    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory

  )

  $Private:NodeArguments = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArguments.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArguments.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:Outdated = @{
    Command          = 'outdated'
    WorkingDirectory = $WorkingDirectory
    NodeArguments    = $NodeArguments
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
    [GenericCompletions(
      {
        return [NodePackageNamedVersion].GetEnumNames()
      }
    )]
    [string]$Version,

    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory

  )

  if ($Version) {
    if ($null -eq [NodePackageNamedVersion]::$Version) {
      if ($Version -match $VERSION_SPEC) {
        [hashtable]$Private:FullVersion = @{
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
    else {
      $Version = [NodePackageNamedVersion]::$Version
    }
  }
  else {
    $Version = [NodePackageNamedVersion]::patch
  }

  $Private:NodeArguments = [List[string]]::new(
    [List[string]]@(
      $Version.ToLowerInvariant()
    )
  )

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArguments.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArguments.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:StepVersion = @{
    Command          = 'version'
    WorkingDirectory = $WorkingDirectory
    NodeArguments    = $NodeArguments
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

    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory

  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $Private:NodeArguments = [List[string]]::new(
    [List[string]]@(
      $Script
    )
  )

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArguments.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArguments.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:RunScript = @{
    Command          = 'run'
    WorkingDirectory = $WorkingDirectory
    NodeArguments    = $NodeArguments
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

    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory

  )

  $Private:NodeArguments = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArguments.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArguments.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:Test = @{
    Command          = 'test'
    WorkingDirectory = $WorkingDirectory
    NodeArguments    = $NodeArguments
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
