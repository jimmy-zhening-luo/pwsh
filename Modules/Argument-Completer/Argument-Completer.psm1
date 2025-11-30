using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

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
    [bool] $flat
    [bool] $useDirectorySeparatorChar
  ) {
    $Container = @{
      Path = $root
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
    $separator = $this.UseDirectorySeparatorChar ? [System.IO.Path]::DirectorySeparatorChar : '/'
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

    if ($separator -ne '/') {
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

class UnitCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Units

  UnitCompletionsAttribute(
    [string] $units
  ) {
    $this.Units = $units
  }

  [IArgumentCompleter] Create() {
    return [UnitCompleter]::new(
      $this.Units
    )
  }
}

class UnitCompleter : IArgumentCompleter {
  [string] $Units

  UnitCompleter(
    [string] $units
  ) {
    if (-not $units) {
      throw [ArgumentException]::new('units')
    }

    $this.Units = $units
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {

    $Local:units = $this.Units -split ',' |
      % { $_.Trim() } |
      % { $_.ToLowerInvariant() } |
      Get-Unique
    $unitMatches = @()
    $resultList = [List[CompletionResult]]::new()

    if ($wordToComplete) {
      $unitMatches = $Local:units |
        ? { $_ -like "$wordToComplete*" }
    }

    if (-not $unitMatches) {
      $unitMatches = $Local:units
    }

    foreach ($unit in $unitMatches) {
      $resultList.Add([CompletionResult]::new($unit))
    }

    return $resultList
  }
}

$ExportableTypes = @(
  [PathCompletionsAttribute]
  [PathCompleter]
  [UnitCompletionsAttribute]
  [UnitCompleter]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
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
