using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [string] $Type
  [bool] $Flat

  PathCompletionsAttribute(
    [string] $root
  ) {
    $this.Root = $root
    $this.Type = ""
    $this.Flat = $false
  }

  PathCompletionsAttribute(
    [string] $root,
    [string] $type
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $false
  }

  PathCompletionsAttribute(
    [string] $root,
    [string] $type,
    [bool] $flat
  ) {
    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
  }

  [IArgumentCompleter] Create() {
    return [PathCompleter]::new(
      $this.Root,
      $this.Type,
      $this.Flat
    )
  }
}

class PathCompleter : IArgumentCompleter {
  [string] $Root
  [string] $Type
  [bool] $Flat

  PathCompleter(
    [string] $root,
    [string] $type,
    [bool] $flat
  ) {
    if (-not $root -or -not (Test-Path -Path $root -PathType Container)) {
      throw [ArgumentException]::new("root")
    }

    if ($type -and -not ($type -eq 'File' -or $type -eq 'Directory')) {
      throw [ArgumentException]::new("type")
    }

    $this.Root = Resolve-Path -Path $root |
      Select-Object -ExpandProperty Path
    $this.Type = $type
    $this.Flat = $flat
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters) {

    $Local:root = $this.Root
    $Local:query = @{
      Path      = $Local:root
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }
    $Local:word = $wordToComplete

    if ($Local:word) {
      $Local:word = $Local:word -replace '[\\\/]+', '\' -replace '^\\', ''
    }

    $Local:subpath = ''
    $Local:leaves = @()
    $resultList = [List[CompletionResult]]::new()

    if ($Local:word) {
      if ($Local:word.EndsWith('\')) {
        $Local:word += '*'
      }

      $Local:subpath = Split-Path $Local:word
      $Local:fragment = Split-Path $Local:word -Leaf

      if ($Local:fragment -eq '*') {
        $Local:fragment = ''
      }

      $Local:path = Join-Path $Local:root $Local:subpath

      if (Test-Path -Path $Local:path -PathType Container) {
        $Local:query.Path = $Local:path
        $Local:query["Filter"] = "$Local:fragment*"
        $Local:leaves = Get-ChildItem @Local:query
      }
    }
    else {
      $Local:leaves = Get-ChildItem @Local:query
    }

    $Local:directories, $Local:files = $Local:leaves.Where(
      { $_.PSIsContainer },
      'Split'
    )
    $Local:directories = $Local:directories |
      Select-Object -ExpandProperty Name
    $Local:files = $Local:files |
      Select-Object -ExpandProperty Name

    if ($Local:subpath -and -not $this.Flat) {
      $Local:directories += ""
    }

    if ($Local:subpath) {
      $Local:directories = $Local:directories |
        % { Join-Path $Local:subpath $_ }
      $Local:files = $Local:files |
        % { Join-Path $Local:subpath $_ }
    }

    if (-not $this.Flat) {
      $Local:directories = $Local:directories |
        % { $_ + "\" }
    }

    $Local:directories = $Local:directories |
      % { $_ -replace '[\\]+', '/' }
    $Local:files = $Local:files |
      % { $_ -replace '[\\]+', '/' }

    foreach ($directory in $Local:directories) {
      $resultList.Add([CompletionResult]::new($directory))
    }
    foreach ($file in $Local:files) {
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
