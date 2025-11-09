using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class PathCompleter : IArgumentCompleter {
  [string] $Root
  [string] $Type

  PathCompleter([string] $root, [string] $type) {
    if (-not $root -or -not (Test-Path -Path $root -PathType Container)) {
      throw [ArgumentException]::new("root")
    }

    if ($type -and -not ($type -eq 'File' -or $type -eq 'Directory')) {
      throw [ArgumentException]::new("type")
    }

    $this.Root = Resolve-Path -Path $root
    $this.Type = $type
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters) {

    $completions = [List[CompletionResult]]::new()
    $Local:root = $this.Root
    $word = $wordToComplete

    $query = @{
      Path      = $Local:root
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }

    if ($word) {
      $word = $word -replace '[\\\/]+', '\' -replace '^\\', ''
    }

    $subpath = ''
    $leaves = $();

    if ($word) {
      if ($word.EndsWith('\')) {
        $word += '*'
      }

      $subpath = Split-Path $word
      $fragment = Split-Path $word -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      $Local:path = Join-Path $Local:root $subpath

      if (Test-Path -Path $Local:path -PathType Container) {
        $query.Path = $Local:path
        $query["Filter"] = "$Local:fragment*"
        $leaves = Get-ChildItem @query
      }
    }
    else {
      $leaves = Get-ChildItem @query |
        Select-Object -ExpandProperty Name |
        % { $completions.Add([CompletionResult]::new($_)) }
    }

    $directories = $leaves |
      ? { $_.PSIsContainer }
    $files = $leaves |
      ? { -not $_.PSIsContainer }

    $directories = $directories |
      Select-Object -ExpandProperty Name
    $files = $files |
      Select-Object -ExpandProperty Name

    if ($subpath) {
      $directories = $directories |
        % { Join-Path $subpath $_ }
      $files = $files |
        % { Join-Path $subpath $_ }
    }

    $directories = $directories |
      % { $_ + "\" }
    $directories = $directories |
      % { $_ -replace '[\\]+', '/' }
    $files = $files |
      % { $_ -replace '[\\]+', '/' }

    foreach ($directory in $directories) {
      $completions.Add([CompletionResult]::new($directory))
    }
    foreach ($file in $files) {
      $completions.Add([CompletionResult]::new($file))
    }

    return $completions
  }
}

class PathCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Root
  [string] $Type

  PathCompletionsAttribute([string] $root, [string] $type) {
    $this.Root = $root
    $this.Type = $type
  }

  [IArgumentCompleter] Create() { return [PathCompleter]::new($this.Root, $this.Type) }
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
