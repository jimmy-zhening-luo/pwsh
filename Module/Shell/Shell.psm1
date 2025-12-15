using namespace System.IO
using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language
using namespace PathCompleter

class PathCompleter : PathCompleterCore {

  PathCompleter(

    [string] $root,

    [PathItemType] $type,

    [bool] $flat,

    [bool] $useNativeDirectorySeparator

  ): base(
    $root,
    $type,
    $flat,
    $useNativeDirectorySeparator
  ) {}

  [List[string]] FindPathCompletion(
    [string] $typedPath,
    [string] $separator
  ) {
    $private:completionPaths = [List[string]]::new()

    [hashtable]$private:isRootContainer = @{
      Path     = $this.Root
      PathType = 'Container'
    }
    if (-not (Test-Path @isRootContainer)) {
      return $completionPaths
    }

    $private:fullRoot = Resolve-Path -Path $this.Root

    [hashtable]$private:nextNode = @{}
    switch ($this.Type) {
      Directory {
        $nextNode.Directory = $True
      }
      File {
        $nextNode.File = $True
      }
    }

    [string]$private:typedAtomicContainer = ''

    if ($typedPath) {
      if ($typedPath.EndsWith([PathCompleter]::NormalDirectorySeparator)) {
        $typedPath += '*'
      }

      $typedAtomicContainer = Split-Path $typedPath
      [string]$private:typedFragment = Split-Path $typedPath -Leaf

      if ($typedFragment -eq '*') {
        $typedFragment = ''
      }

      [string]$private:path = Join-Path $fullRoot $typedAtomicContainer

      if (Test-Path -Path $path -PathType Container) {
        $nextNode.Path = $path
        $nextNode['Filter'] = "$typedFragment*"
      }
    }

    if (-not $nextNode.Path) {
      $nextNode.Path = $fullRoot
    }

    [FileSystemInfo[]]$private:leaves = Get-ChildItem @nextNode

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

    if ($typedAtomicContainer -and -not $this.Flat) {
      $directories += ''
    }

    if ($typedAtomicContainer) {
      $directories = $directories |
        ForEach-Object {
          Join-Path $typedAtomicContainer $PSItem
        }
      $files = $files |
        ForEach-Object {
          Join-Path $typedAtomicContainer $PSItem
        }
    }

    if (-not $this.Flat) {
      $directories = $directories |
        ForEach-Object {
          $PSItem + [PathCompleter]::NormalDirectorySeparator
        }
    }

    $directories = $directories -replace [regex][PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::NormalDirectorySeparator
    $files = $files -replace [regex][PathCompleter]::DuplicateDirectorySeparatorPattern, [PathCompleter]::NormalDirectorySeparator

    if ($separator -ne [PathCompleter]::NormalDirectorySeparator) {
      $directories = $directories -replace [regex][PathCompleter]::NormalDirectorySeparator, [PathCompleter]::EasyDirectorySeparator
      $files = $files -replace [regex][PathCompleter]::NormalDirectorySeparator, [PathCompleter]::EasyDirectorySeparator
    }

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
  [bool]$Private:FileLike = $HasSubpath -and -not (
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

New-Alias cl Clear-Line

$ExportableTypes = @(
  [PathCompletionsAttribute]
)

$TypeAcceleratorsClass = [PSObject].Assembly.GetType('System.Management.Automation.TypeAccelerators')

$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get

foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    [string]$Message = @(
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

foreach ($Type in $ExportableTypes) {
  $TypeAcceleratorsClass::Add($Type.FullName, $Type)
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  foreach ($Type in $ExportableTypes) {
    $TypeAcceleratorsClass::Remove($Type.FullName)
  }
}.GetNewClosure()
