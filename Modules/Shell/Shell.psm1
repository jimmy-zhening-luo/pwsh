using module GenericArgumentCompleter

using namespace System.IO
using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

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

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [string] $Type
  [bool] $Flat
  [bool] $UseNativeDirectorySeparator

  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = ''
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $false
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type,
    [bool] $flat
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type,
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

class PathCompleter : GenericCompleterBase, IArgumentCompleter {
  [string] $Root
  [string] $Type
  [bool] $Flat
  [bool] $UseNativeDirectorySeparator

  PathCompleter(
    [string] $root,
    [string] $type,
    [bool] $flat,
    [bool] $useNativeDirectorySeparator
  ) {
    if (-not $root -or -not (Test-Path -Path $root -PathType Container)) {
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

    $Local:root = Resolve-Path -Path $this.Root
    $separator = $this.UseNativeDirectorySeparator ? [Path]::DirectorySeparatorChar : '/'
    $query = @{
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }
    $currentText = $wordToComplete ? $wordToComplete -match "^'(?<CurrentText>.*)'$" ? $Matches.CurrentText -replace "''", "'" : $wordToComplete : ''
    $currentPathText = $currentText -replace '[\\\/]', '\'
    $currentDirectoryText = ''

    if ($currentPathText) {
      if ($currentPathText.EndsWith('\')) {
        $currentPathText += '*'
      }

      $currentDirectoryText = Split-Path $currentPathText
      $fragment = Split-Path $currentPathText -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      $path = Join-Path $Local:root $currentDirectoryText

      if (Test-Path -Path $path -PathType Container) {
        $query.Path = $path
        $query['Filter'] = "$fragment*"
        $leaves = Get-ChildItem @query
      }
    }

    if (-not $query.Path) {
      $query.Path = $Local:root
    }

    $leaves = @()
    $leaves += Get-ChildItem @query
    $directories, $files = $leaves.Where(
      { $PSItem.PSIsContainer },
      'Split'
    )
    $directories = $directories |
      Select-Object -ExpandProperty Name
    $files = $files |
      Select-Object -ExpandProperty Name

    if ($currentDirectoryText -and -not $this.Flat) {
      $directories += ''
    }

    if ($currentDirectoryText) {
      $directories = $directories |
        ForEach-Object { Join-Path $currentDirectoryText $PSItem }
      $files = $files |
        ForEach-Object { Join-Path $currentDirectoryText $PSItem }
    }

    if (-not $this.Flat) {
      $directories = $directories |
        ForEach-Object { $PSItem + '\' }
    }

    $items = [List[string]]::new()

    if ($directories) {
      $items.AddRange([List[string]]$directories)
    }
    if ($files) {
      $items.AddRange([List[string]]$files)
    }

    if ($separator -ne '\') {
      $items = $items -replace '(?>[\\]+)', '/'
    }

    return [PathCompleter]::CreateCompletion($items)
  }
}

$ExportableTypes = @(
  [PathCompleter]
  [PathCompletionsAttribute]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    $Message = @(
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

function Format-Path {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Separator,
    [switch]$LeadingRelative,
    [switch]$Trailing
  )

  $AlignedPath = $Path -replace '[\\\/]', '\'
  $TrimmedPath = $AlignedPath -replace '(?<!^)(?>\\+)', '\'

  if ($LeadingRelative) {
    $TrimmedPath = $TrimmedPath -replace '^\.(?>\\+)', ''
  }

  if ($Trailing) {
    $TrimmedPath = $TrimmedPath -replace '(?>\\+)$', ''
  }

  $Separator -and $Separator -ne '\' ? $TrimmedPath -replace '\\', $Separator : $TrimmedPath
}

function Trace-RelativePath {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [Path]::GetRelativePath($Path, $Location) -match '^(?>[.\\]*)$'
}

function Merge-RelativePath {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [Path]::GetRelativePath($Location, $Path)
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
      $Relative = @{
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
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~(?>\\*)', ''

    if ($Location) {
      $Relative = @{
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

  $Container = @{
    Path     = $Location
    PathType = 'Container'
  }
  if (-not (Test-Path @Container)) {
    return $False
  }

  $FullLocation = (Resolve-Path -Path $Location).Path
  $FullPath = Join-Path $FullLocation $Path
  $HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch '^\\*$'
  $FileLike = $HasSubpath -and -not (
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

  $Item = @{
    Path     = $FullPath
    PathType = $File ? 'Leaf' : 'Container'
  }
  if ($New) {
    (Test-Path @Item -IsValid) -and -not (Test-Path @Item)
  }
  else {
    Test-Path @Item
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
    throw "Invalid path '$Path': " + ($PSBoundParameters | ConvertTo-Json -EnumsAsStrings)
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
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~(?>\\*)', ''

    if ($Location) {
      $Path = Merge-RelativePath -Path (
        Join-Path $HOME $Path
      ) -Location $Location
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  $FullLocation = (Resolve-Path -Path $Location).Path
  $FullPath = Join-Path $FullLocation $Path

  $New ? $FullPath : (
    Resolve-Path -Path $FullPath -Force
  ).Path
}
