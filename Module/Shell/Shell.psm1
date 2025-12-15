using namespace System.IO
using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language
using namespace Completer
using namespace PathCompleter

class PathCompleter : CompleterBase {

  [string] $Root
  [PathItemType] $Type
  [PathProvider] $Provider
  [bool] $Flat
  [bool] $UseNativeDirectorySeparator

  PathCompleter(

    [string] $root,
    [PathItemType] $type,
    [PathProvider] $provider,
    [bool] $flat,
    [bool] $useNativeDirectorySeparator

  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = $provider
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $useNativeDirectorySeparator
  }

  [List[string]] FulfillCompletion(
    [string] $parameterName,
    [string] $wordToComplete,
    [IDictionary] $fakeBoundParameters
  ) {
    [hashtable]$private:matchChild = @{}

    switch ($this.Type) {
      Directory {
        $matchChild.Directory = $True
      }
      File {
        $matchChild.File = $True
      }
    }

    [string]$private:currentValue = [PathCompleter]::Unescape($wordToComplete)

    [string]$private:currentPathValue = $currentValue -replace [regex][PathSyntax]::EasyDirectorySeparatorPattern, [PathSyntax]::NormalDirectorySeparator -replace [regex][PathSyntax]::DuplicateDirectorySeparatorPattern, [PathSyntax]::NormalDirectorySeparator

    [string]$private:currentDirectoryValue = ''

    if ($currentPathValue) {
      if (
        $currentPathValue.EndsWith(
          [PathSyntax]::NormalDirectorySeparator
        )
      ) {
        $currentPathValue += '*'
      }

      $currentDirectoryValue = Split-Path $currentPathValue
      [string]$private:fragment = Split-Path $currentPathValue -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      [string]$private:path = Join-Path $this.Root $currentDirectoryValue

      if (Test-Path -Path $path -PathType Container) {
        $matchChild.Path = $path
        $matchChild['Filter'] = "$fragment*"
      }
    }

    if (-not $matchChild.Path) {
      $matchChild.Path = $this.Root
    }

    [FileSystemInfo[]]$private:leaves = Get-ChildItem @matchChild

    [FileSystemInfo[]]$private:containers, [FileSystemInfo[]]$private:children = $leaves.Where(
      {
        $PSItem.PSIsContainer
      },
      'Split'
    )

    [string[]]$private:directories = $containers |
      Select-Object -ExpandProperty Name
    [string[]]$private:files = $children |
      Select-Object -ExpandProperty Name

    if ($currentDirectoryValue -and -not $this.Flat) {
      $directories += ''
    }

    if ($currentDirectoryValue) {
      $directories = $directories |
        ForEach-Object {
          Join-Path $currentDirectoryValue $PSItem
        }
      $files = $files |
        ForEach-Object {
          Join-Path $currentDirectoryValue $PSItem
        }
    }

    if (-not $this.Flat) {
      $directories = $directories |
        ForEach-Object {
          $PSItem + [PathSyntax]::NormalDirectorySeparator
        }
    }

    $directories = $directories -replace [regex][PathSyntax]::DuplicateDirectorySeparatorPattern, [PathSyntax]::NormalDirectorySeparator
    $files = $files -replace [regex][PathSyntax]::DuplicateDirectorySeparatorPattern, [PathSyntax]::NormalDirectorySeparator

    [string]$private:separator = $this.UseNativeDirectorySeparator ? [Path]::DirectorySeparatorChar : [PathSyntax]::EasyDirectorySeparator
    if ($separator -ne [PathSyntax]::NormalDirectorySeparator) {
      $directories = $directories -replace [regex][PathSyntax]::DuplicateDirectorySeparatorPattern, $separator
      $files = $files -replace [regex][PathSyntax]::DuplicateDirectorySeparatorPattern, $separator
    }

    $private:completionPaths = [List[string]]::new()
    if ($directories) {
      $completionPaths.AddRange(
        [List[string]]$directories
      )
    }
    if ($files) {
      $completionPaths.AddRange(
        [List[string]]$files
      )
    }

    return $completionPaths
  }
}

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [PathItemType] $Type
  [PathProvider] $Provider
  [bool] $Flat
  [bool] $UseNativeDirectorySeparator

  PathCompletionsAttribute() {}
  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = [PathItemType]::Any
    $this.Provider = [PathProvider]::Any
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = [PathProvider]::Any
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
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
    $this.UseNativeDirectorySeparator = $false
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
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [PathProvider] $provider,
    [bool] $flat,
    [bool] $useNativeDirectorySeparator
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Provider = $provider
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $useNativeDirectorySeparator
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

    return [PathCompleter]::new(
      $private:root,
      $this.Type,
      $this.Provider,
      $this.Flat,
      $this.UseNativeDirectorySeparator
    )
  }
}

$TYPES = @(
  [PathCompletionsAttribute]
)

$TypeAccelerators = [PSObject].Assembly.GetType('System.Management.Automation.TypeAccelerators')
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

  $Path = [PathSyntax]::Format(
    $Path,
    '',
    $True
  )
  $Location = [PathSyntax]::Format($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][PathSyntax]::DescendantPattern
      ) {
        $Path = [Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][PathSyntax]::TildeRootedPattern) {
    $Path = $Path -replace [regex][PathSyntax]::TildeRootPattern, ''

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][PathSyntax]::DescendantPattern
      ) {
        $Path = [Path]::GetRelativePath(
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
      [PathSyntax]::NormalDirectorySeparator
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

  $Path = [PathSyntax]::Format(
    $Path,
    '',
    $True
  )
  $Location = [PathSyntax]::Format($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = [Path]::GetRelativePath(
        $Location,
        $Path
      )
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][PathSyntax]::TildeRootedPattern) {
    $Path = $Path -replace [regex][PathSyntax]::TildeRootPattern, ''

    if ($Location) {
      $Path = [Path]::GetRelativePath(
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
