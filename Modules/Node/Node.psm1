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
    # Node project root path to be resolved
    [string]$Path = $PWD.Path
  )

  $IsNode = @{
    Path     = Join-Path $Path package.json
    PathType = 'Leaf'
  }
  if (Test-Path @IsNode) {
    $Prefix = (Resolve-Path $Path).Path

    $Prefix -eq $PWD.Path ? '' : $Prefix
  }
  else {
    throw "Path '$Path' is not a Node project directory."
  }
}

New-Alias npc Node\Clear-PackageCache
<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node package cache.
.DESCRIPTION
This function is an alias for 'npm cache clean --force'.
.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-PackageCache {
  & npm cache clean --force @args
}

New-Alias npo Node\Compare-Package
<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node project.
.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $Path]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-Package {
  param(
    [PathCompletions(
      '~\code',
      'Directory',
      $true
    )]
    # Node project root
    [string]$Path
  )

  $Prefix = Resolve-NodeProject @PSBoundParameters
  $Local:args = $args

  if ($Prefix) {
    $Local:args = '--prefix', $Prefix + $Local:args
  }

  & npm outdated @Local:args
}

New-Alias nr Node\Invoke-Script
<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node project's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm run [script] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Invoke-Script {
  param(
    [Parameter(Mandatory)]
    [Alias('Run')]
    # Name of the npm script to run
    [string]$Script,
    [PathCompletions(
      '~\code',
      'Directory',
      $true
    )]
    # Node project root
    [string]$Path
  )

  $Local:args = $args

  if ($Path.StartsWith(('-'))) {
    $Local:args = , $Path + $Local:args
    $Path = ''
  }

  $Prefix = Resolve-NodeProject @PSBoundParameters

  if ($Prefix) {
    $Local:args = '--prefix', $Prefix + $Local:args
  }

  & npm run $Script @Local:args
}
