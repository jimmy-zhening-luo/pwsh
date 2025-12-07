<#
.SYNOPSIS
Resolve a Node project at its root directory.
.DESCRIPTION
This function resolves the supplied path to a qualified, rooted path if it is a Node project root. If the supplied path is not a Node project root, an error is thrown.
.LINK
https://docs.npmjs.com/cli/commands
#>
function Resolve-NodeProject {
  [OutputType([string])]
  param(
    [PathCompletions('.', 'Directory')]
    # Node project root path to be resolved
    [string]$Path = $PWD.Path,
    [switch]$OmitPrefix
  )

  $IsNodeProject = @{
    Path     = Join-Path $Path package.json
    PathType = 'Leaf'
  }
  if (Test-Path @IsNodeProject) {
    $Project = (Resolve-Path $Path).Path

    $Project -eq $PWD.Path ? '' : $OmitPrefix ? $Project : "--prefix=$Project"
  }
  else {
    throw "Path '$Path' is not a Node project directory."
  }
}

New-Alias ncc Node\Clear-NodeModuleCache
<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node package cache.
.DESCRIPTION
This function is an alias for 'npm cache clean --force'.
.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {
  & npm cache clean --force @args
}

New-Alias no Node\Compare-NodeModule
<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node project.
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
    # Node project root
    [string]$Path
  )

  $NodeArguments = $args
  $NodeArguments = , (Resolve-NodeProject @PSBoundParameters) + $NodeArguments

  & npm outdated @NodeArguments
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to increment the package version of the current Node project.
.DESCRIPTION
This function is an alias for 'npm version [--prefix $Path] [version=patch]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodeProjectVersion {
  param(
    # New package version, default 'patch'
    [GenericCompletions('patch,minor,major,prerelease,preminor,premajor')]
    [string]$Version = 'patch',
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node project root
    [string]$Path
  )

  $NodeArguments = $args
  $NodeArguments = , (Resolve-NodeProject @PSBoundParameters) + $NodeArguments

  $NamedVersion = @(
    'patch'
    'minor'
    'major'
    'prerelease'
    'preminor'
    'premajor'
  )

  if ($Version -notin $NamedVersion) {
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

  $NodeArguments += $Version.ToLowerInvariant()

  & npm version @NodeArguments
}

New-Alias nr Node\Invoke-NodeProjectScript
<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node project's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $Path] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodeProjectScript {
  param(
    # Name of the npm script to run
    [string]$Script,
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node project root
    [string]$Path
  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $NodeArguments = $args

  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $Path = ''
  }

  $NodeArguments = , (Resolve-NodeProject @PSBoundParameters) + $NodeArguments

  & npm run $Script @NodeArguments
}

New-Alias nt Node\Test-NodeProject
<#
.SYNOPSIS
Use Node Package Manager (npm) to run the 'test' script defined in a Node project's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm test [--prefix $Path] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodeProject {
  param(
    [PathCompletions(
      '~\code',
      'Directory',
      $True
    )]
    # Node project root
    [string]$Path
  )

  $NodeArguments = $args

  if ($Path.StartsWith(('-'))) {
    $NodeArguments = , $Path + $NodeArguments
    $Path = ''
  }

  $NodeArguments = , (Resolve-NodeProject @PSBoundParameters) + $NodeArguments

  & npm test @NodeArguments
}
