using module GenericArgumentCompleter

using namespace System.IO
using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
New-Alias cl Clear-Line
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

enum PathItemType {
  Any
  File
  Directory
}

class PathCompleter : GenericCompleterBase, IArgumentCompleter {

  static [string] $EasyDirectorySeparator = '/'

  static [string] $NormalDirectorySeparator = '\'

  static [regex] $DuplicateDirectorySeparatorPattern = [regex]'(?<!^)\\+'

  [string] $Root

  [PathItemType] $Type

  [bool] $Flat

  [bool] $UseNativeDirectorySeparator

  PathCompleter(

    [string] $root,

    [PathItemType] $type,

    [bool] $flat,

    [bool] $useNativeDirectorySeparator

  ) {
    [hashtable]$private:rootIsContainer = @{
      Path     = $root
      PathType = 'Container'
    }
    if (-not $root -or -not (Test-Path @rootIsContainer)) {
      throw [ArgumentException]::new('root')
    }

    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $useNativeDirectorySeparator
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
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

    $private:root = Resolve-Path -Path $this.Root

    [string]$private:currentValue = [PathCompleter]::Unescape($wordToComplete)

    [string]$private:currentPathValue = $currentValue -replace [regex][PathCompleter]::EasyDirectorySeparator, [PathCompleter]::NormalDirectorySeparator -replace [PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::NormalDirectorySeparator

    [string]$private:currentDirectoryValue = ''

    if ($currentPathValue) {
      if ($currentPathValue.EndsWith([PathCompleter]::NormalDirectorySeparator)) {
        $currentPathValue += '*'
      }

      $currentDirectoryValue = Split-Path $currentPathValue
      [string]$private:fragment = Split-Path $currentPathValue -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      [string]$private:path = Join-Path $private:root $currentDirectoryValue

      if (Test-Path -Path $path -PathType Container) {
        $matchChild.Path = $path
        $matchChild['Filter'] = "$fragment*"
      }
    }

    if (-not $matchChild.Path) {
      $matchChild.Path = $private:root
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
          $PSItem + [PathCompleter]::NormalDirectorySeparator
        }
    }

    $directories = $directories -replace [PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::NormalDirectorySeparator
    $files = $files -replace [PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::NormalDirectorySeparator

    [string]$private:separator = $this.UseNativeDirectorySeparator ? [Path]::DirectorySeparatorChar : [PathCompleter]::EasyDirectorySeparator
    if ($separator -ne [PathCompleter]::NormalDirectorySeparator) {
      $directories = $directories -replace [PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::EasyDirectorySeparator
      $files = $files -replace [PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::EasyDirectorySeparator
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
    $private:completionValues = [List[string]]::new(
      [List[string]]$completionPaths
    )

    return [PathCompleter]::CreateCompletionResult(
      $completionValues
    )
  }
}

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [PathItemType] $Type
  [bool] $Flat
  [bool] $UseNativeDirectorySeparator

  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = [PathItemType]::Any
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [bool] $flat
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [PathItemType] $type,
    [bool] $flat,
    [bool] $useNativeDirectorySeparator
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $useNativeDirectorySeparator
  }

  [IArgumentCompleter] Create() {
    return [PathCompleter]::new(
      $this.Root,
      $this.Type,
      $this.Flat,
      $this.UseNativeDirectorySeparator
    )
  }
}

$ExportableTypes = @(
  [PathItemType]
  [PathCompleter]
  [PathCompletionsAttribute]
)

$TypeAcceleratorsClass = [PSObject].Assembly.GetType('System.Management.Automation.TypeAccelerators')

$Private:ExistingTypeAccelerators = $TypeAcceleratorsClass::Get

foreach ($Private:Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    [string]$Private:Message = @(
      "Unable to register type accelerator '$($Type.FullName)'"
      'Accelerator already exists.'
    ) -join ' - '

    throw [System.Management.Automation.ErrorRecord]::new(
      [System.InvalidOperationException]::new($Message),
      'TypeAcceleratorAlreadyExists',
      [System.Management.Automation.ErrorCategory]::InvalidOperation,
      $Type.FullName
    )
  }
}

foreach ($Private:Type in $ExportableTypes) {
  $TypeAcceleratorsClass::Add($Type.FullName, $Type)
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  foreach ($Private:Type in $ExportableTypes) {
    $TypeAcceleratorsClass::Remove($Type.FullName)
  }
}.GetNewClosure()

function Format-Path {

  [OutputType([string])]

  param(

    [string]$Path,
    [string]$Separator,
    [switch]$LeadingRelative,
    [switch]$Trailing

  )

  $Private:AlignedPath = $Path -replace [regex]'[\\\/]', '\'
  $Private:TrimmedPath = $AlignedPath -replace [regex]'(?<!^)\\+', '\'

  if ($LeadingRelative) {
    $TrimmedPath = $TrimmedPath -replace [regex]'^\.(?>\\+)', ''
  }

  if ($Trailing) {
    $TrimmedPath = $TrimmedPath -replace [regex]'(?>\\+)$', ''
  }

  return $Separator -and $Separator -ne '\' ? $TrimmedPath -replace [regex]'\\', $Separator : $TrimmedPath
}

function Trace-RelativePath {

  [OutputType([bool])]

  param(

    [string]$Path,

    [string]$Location

  )

  return [Path]::GetRelativePath($Path, $Location) -match [regex]'^(?>[.\\]*)$'
}

function Merge-RelativePath {

  [OutputType([string])]

  param(

    [string]$Path,

    [string]$Location

  )

  return [Path]::GetRelativePath($Location, $Path)
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

  $Path = Format-Path -Path $Path -LeadingRelative
  $Location = Format-Path -Path $Location

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      [hashtable]$Private:Relative = @{
        Path     = $Path
        Location = $Location
      }
      if (Trace-RelativePath @Relative) {
        $Path = Merge-RelativePath @Relative
      }
      else {
        return $False
      }
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex]'^~(?=\\|$)') {
    $Path = $Path -replace [regex]'^~(?>\\*)', ''

    if ($Location) {
      [hashtable]$Private:Relative = @{
        Path     = Join-Path $HOME $Path
        Location = $Location
      }
      if (Trace-RelativePath @Relative) {
        $Path = Merge-RelativePath @Relative
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

  [string]$Private:FullLocation = Resolve-Path -Path $Location
  [string]$Private:FullPath = Join-Path $FullLocation $Path
  [bool]$Private:HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch [regex]'^\\*$'
  $Private:FileLike = $HasSubpath -and -not (
    $FullPath.EndsWith('\') -or $FullPath.EndsWith('..')
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

  $Path = Format-Path -Path $Path -LeadingRelative
  $Location = Format-Path -Path $Location

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex]'^~(?=\\|$)') {
    $Path = $Path -replace [regex]'^~(?>\\*)', ''

    if ($Location) {
      [hashtable]$Private:RelativePath = @{
        Path     = Join-Path $HOME $Path
        Location = $Location
      }
      $Path = Merge-RelativePath @RelativePath
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD
  }

  [string]$Private:FullLocation = Resolve-Path -Path $Location
  [string]$Private:FullPath = Join-Path $FullLocation $Path

  if ($New) {
    return $FullPath
  }
  else {
    return [string](Resolve-Path -Path $FullPath -Force)
  }
}
