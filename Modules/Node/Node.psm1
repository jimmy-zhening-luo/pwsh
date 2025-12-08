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

  $IsNodePackage = @{
    Path     = Microsoft.PowerShell.Management\Join-Path ($Path ? $Path : $PWD) package.json
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

  $IsNodePackage = @{
    Path = $Path
  }
  if (Test-NodePackageDirectory @IsNodePackage) {
    $Package = ($Path ? (Microsoft.PowerShell.Management\Resolve-Path $Path) : $PWD).Path

    $OmitPrefix ? $Package : "--prefix=$Package"
  }
  else {
    throw "Path '$Path' is not a Node package directory."
  }
}

New-Alias no Node\Invoke-Node
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

New-Alias n Node\Invoke-NodePackage
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
  & npm.ps1 @args
}

New-Alias nx Node\Invoke-NodeExecutable
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

New-Alias ncc Node\Clear-NodeModuleCache
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

New-Alias npo Node\Compare-NodeModule
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
    if ($Version -match '^v?(?<Major>\d+)(?>\.(?<Minor>\d*)(?>\.(?<Patch>\d*))?)?(?>-(?<Pre>\w+(?>\.\d+)?))?$') {
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

New-Alias nr Node\Invoke-NodePackageScript
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

New-Alias nt Node\Test-NodePackage
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
