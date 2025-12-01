using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.IO
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

function Format-Path {
  param(
    [string]$Path,
    [switch]$Leading,
    [switch]$Trailing
  )

  $TrimmedPath = $Path -replace '[\\\/]+', '\'

  if ($Leading) {
    $TrimmedPath = $Path -replace '^(?>\.\\+)', ''
  }

  if ($Trailing) {
    $TrimmedPath = $Path -replace '\\+$', ''
  }

  $TrimmedPath
}

function Trace-RelativePath {
  param(
    [string]$Path,
    [string]$Location
  )
  [Path]::GetRelativePath($Path, $Location) -match '^[.\\]*$'
}

function Merge-RelativePath {
  param(
    [string]$Path,
    [string]$Location
  )

  [Path]::GetRelativePath($Location, $Path)
}

function Test-Item {
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  $Location = Format-Path -Path $Location
  $Path = Format-Path -Path $Path -Leading

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
    $Path = $Path -replace '^~\\*', ''

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

  if (-not (Test-Path -Path $Location -PathType Container)) {
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
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not (Test-Item @PSBoundParameters)) {
    throw "Path '$Path' fails to meet criteria: " + ($PSBoundParameters | ConvertTo-Json)
  }

  $Location = Format-Path -Path $Location
  $Path = Format-Path -Path $Path -Leading

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~\\*', ''

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

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [string] $Type
  [bool] $Flat
  [bool] $UseDirectorySeparatorChar

  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = ''
    $this.Flat = $false
    $this.UseDirectorySeparatorChar = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $false
    $this.UseDirectorySeparatorChar = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type,
    [bool] $flat
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseDirectorySeparatorChar = $false
  }
  PathCompletionsAttribute(
    [string] $root,
    [string] $type,
    [bool] $flat,
    [bool] $useDirectorySeparatorChar
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseDirectorySeparatorChar = $useDirectorySeparatorChar
  }
  [IArgumentCompleter] Create() {
    return [PathCompleter]::new(
      $this.Root,
      $this.Type,
      $this.Flat,
      $this.UseDirectorySeparatorChar
    )
  }
}

class PathCompleter : IArgumentCompleter {
  [string] $Root
  [string] $Type
  [bool] $Flat
  [bool] $UseDirectorySeparatorChar

  PathCompleter(
    [string] $root,
    [string] $type,
    [bool] $flat,
    [bool] $useDirectorySeparatorChar
  ) {
    $Container = @{
      Path     = $root
      PathType = 'Container'
    }

    if (-not $root -or -not (Test-Path @Container)) {
      throw [ArgumentException]::new('root')
    }

    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseDirectorySeparatorChar = $useDirectorySeparatorChar
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {
    $Local:root = (Resolve-Path -Path $this.Root).Path
    $separator = $this.UseDirectorySeparatorChar ? [Path]::DirectorySeparatorChar : '/'
    $query = @{
      Path      = $Local:root
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }
    $word = $wordToComplete -replace '[\\\/]+', '\' -replace '^\\', ''
    $leaves = @()
    $subpath = ''
    $resultList = [List[CompletionResult]]::new()


    if ($word) {
      if ($word.EndsWith('\')) {
        $word += '*'
      }

      $subpath = Split-Path $word
      $fragment = Split-Path $word -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      $path = Join-Path $Local:root $subpath

      if (Test-Path -Path $path -PathType Container) {
        $query.Path = $path
        $query['Filter'] = "$fragment*"
        $leaves = Get-ChildItem @query
      }
    }
    else {
      $leaves = Get-ChildItem @query
    }

    $directories, $files = $leaves.Where(
      { $_.PSIsContainer },
      'Split'
    )
    $directories = $directories.Name
    $files = $files.Name

    if ($subpath -and -not $this.Flat) {
      $directories += ''
    }

    if ($subpath) {
      $directories = $directories |
        % { Join-Path $subpath $_ }
      $files = $files |
        % { Join-Path $subpath $_ }
    }

    if (-not $this.Flat) {
      $directories = $directories |
        % { $_ + '\' }
    }

    if ($false) {
      $directories = $directories |
        % { $_ -replace '[\\]+', $separator }
      $files = $files |
        % { $_ -replace '[\\]+', $separator }
    }

    foreach ($directory in $directories) {
      $resultList.Add([CompletionResult]::new($directory))
    }
    foreach ($file in $files) {
      $resultList.Add([CompletionResult]::new($file))
    }

    return $resultList
  }
}

$ExportableTypes = @(
  [PathCompletionsAttribute]
  [PathCompleter]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [ErrorRecord]::new(
      [InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
      'TypeAcceleratorAlreadyExists',
      [ErrorCategory]::InvalidOperation,
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
