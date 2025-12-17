using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language
using namespace Typed
using namespace Completer
using namespace Completer.PathCompleter

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [PathItemType] $Type
  [PathProvider] $Provider
  [bool] $Flat
  [bool] $UseNativePathSeparator

  PathCompletionsAttribute() {}
  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = [PathItemType]::Any
    $this.Provider = [PathProvider]::Any
    $this.Flat = $false
    $this.UseNativePathSeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = [PathProvider]::Any
    $this.Flat = $false
    $this.UseNativePathSeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [PathProvider] $provider
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = $provider
    $this.Flat = $false
    $this.UseNativePathSeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [PathProvider] $provider,
    [bool] $flat
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = $provider
    $this.Flat = $flat
    $this.UseNativePathSeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [PathProvider] $provider,
    [bool] $flat,
    [bool] $useNativePathSeparator
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = $provider
    $this.Flat = $flat
    $this.UseNativePathSeparator = $useNativePathSeparator
  }

  [IArgumentCompleter] Create() {
    [hashtable]$private:isRootContainer = @{
      Path     = $this.Root
      PathType = 'Container'
    }
    if (-not $this.Root -or -not (Test-Path @isRootContainer)) {
      throw [ArgumentException]::new('root')
    }

    [string]$private:root = (Resolve-Path -Path $this.Root).Path

    return [PathCompleter.PathCompleter]::new(
      $private:root,
      $this.Type,
      $this.Flat,
      $this.UseNativePathSeparator
    )
  }
}

$TYPES = @(
  [PathCompletionsAttribute]
)

$TypeAccelerators = [psobject].Assembly.GetType('System.Management.Automation.TypeAccelerators')
$ExistingTypes = $TypeAccelerators::Get
foreach ($Private:Type in $TYPES) {
  if ($Type.FullName -in $ExistingTypes.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [System.InvalidOperationException]::new(
        [string]"Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."
      ),
      'TypeAcceleratorAlreadyExists',
      [System.Management.Automation.ErrorCategory]::InvalidOperation,
      $Type.FullName
    )
  }
}
foreach ($Private:Type in $TYPES) {
  $TypeAccelerators::Add($Type.FullName, $Type)
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  foreach ($Private:Type in $TYPES) {
    $TypeAccelerators::Remove($Type.FullName)
  }
}.GetNewClosure()

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {

  [OutputType([void])]

  param(

    [PathCompletions('.')]
    [string]$Path

  )

  if ($Path -or $args) {
    Clear-Content @PSBoundParameters @args
  }
  else {
    Clear-Host
  }
}

function Test-Item {

  [OutputType([bool])]

  param(

    [string]$Path,

    [string]$Location,

    [switch]$File,

    [switch]$New,

    [switch]$RequireSubpath

  )

  $Path = [TypedPath]::Format(
    $Path,
    '',
    $True
  )
  $Location = [TypedPath]::Format($Location)

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [System.IO.Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
      ) {
        $Path = [System.IO.Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][TypedPath]::IsPathTildeRootedPattern) {
    $Path = $Path -replace [regex][TypedPath]::TildeRootPattern, ''

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [System.IO.Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
      ) {
        $Path = [System.IO.Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  [hashtable]$Private:Container = @{
    Path     = $Location
    PathType = 'Container'
  }
  if (-not (Test-Path @Container)) {
    return $False
  }

  [string]$Private:FullLocation = (Resolve-Path -Path $Location).Path
  [string]$Private:FullPath = Join-Path $FullLocation $Path
  [bool]$Private:HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch [regex]'^\\*$'
  [bool]$Private:FileLike = $HasSubpath -and -not (
    $FullPath.EndsWith(
      [TypedPath]::PathSeparator
    ) -or $FullPath.EndsWith('..')
  )

  if (-not $HasSubpath) {
    return -not (
      $RequiresSubpath -or $File -or $New
    )
  }

  if ($File -and -not $FileLike) {
    return $False
  }

  [hashtable]$Private:Item = @{
    Path     = $FullPath
    PathType = $File ? 'Leaf' : 'Container'
  }
  if ($New) {
    return (Test-Path @Item -IsValid) -and -not (Test-Path @Item)
  }
  else {
    return Test-Path @Item
  }
}

function Resolve-Item {

  [OutputType([string])]

  param(

    [string]$Path,

    [string]$Location,

    [switch]$File,

    [switch]$New,

    [switch]$RequireSubpath

  )

  if (-not (Test-Item @PSBoundParameters)) {
    throw "Invalid path '$Path': " + (
      $PSBoundParameters | ConvertTo-Json -EnumsAsStrings
    )
  }

  $Path = [TypedPath]::Format(
    $Path,
    '',
    $True
  )
  $Location = [TypedPath]::Format($Location)

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = [System.IO.Path]::GetRelativePath(
        $Location,
        $Path
      )
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][TypedPath]::IsPathTildeRootedPattern) {
    $Path = $Path -replace [regex][TypedPath]::TildeRootPattern, ''

    if ($Location) {
      $Path = [System.IO.Path]::GetRelativePath(
        $Location,
        (Join-Path $HOME $Path)
      )
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  [string]$Private:FullLocation = (Resolve-Path -Path $Location).Path
  [string]$Private:FullPath = Join-Path $FullLocation $Path

  if ($New) {
    return $FullPath
  }
  else {
    return [string](Resolve-Path -Path $FullPath -Force).Path
  }
}

New-Alias cl Clear-Line
