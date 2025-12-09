class PathCompletionsAttribute : System.Management.Automation.ArgumentCompleterAttribute, System.Management.Automation.IArgumentCompleterFactory {
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

  [System.Management.Automation.IArgumentCompleter] Create() {
    return [PathCompleter]::new(
      $this.Root,
      $this.Type,
      $this.Flat,
      $this.UseNativeDirectorySeparator
    )
  }
}

class PathCompleter : System.Management.Automation.IArgumentCompleter {
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
      throw [System.ArgumentException]::new('root')
    }

    $this.Root = $root
    $this.Type = $type
    $this.Flat = $flat
    $this.UseNativeDirectorySeparator = $useNativeDirectorySeparator
  }

  [System.Collections.Generic.IEnumerable[System.Management.Automation.CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [System.Management.Automation.Language.CommandAst] $commandAst,
    [System.Collections.IDictionary] $fakeBoundParameters
  ) {

    $Local:root = Resolve-Path -Path $this.Root
    $separator = $this.UseNativeDirectorySeparator ? [System.IO.Path]::DirectorySeparatorChar : '/'
    $query = @{
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }
    $currentText = $wordToComplete ? $wordToComplete -match "^'(?<CurrentText>.*)'$" ? $Matches.CurrentText -replace "''", "'" : $wordToComplete : ''
    $currentPathText = $currentText -replace '[\\\/]', '\'
    $currentDirectoryText = ''
    $resultList = [System.Collections.Generic.List[System.Management.Automation.CompletionResult]]::new()

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

    $items = @()

    if ($directories) {
      $items += $directories
    }
    if ($files) {
      $items += $files
    }

    if ($separator -ne '\') {
      $items = $items -replace '(?>[\\]+)', '/'
    }

    foreach ($item in $items) {
      $string = [System.Management.Automation.Language.CodeGeneration]::EscapeSingleQuotedStringContent($item)
      $completion = $string -match '\s' ? "'" + $string + "'" : $string

      $resultList.Add(
        [System.Management.Automation.CompletionResult]::new(
          $completion
        )
      )
    }

    return $resultList
  }
}

$ExportableTypes = @(
  [PathCompletionsAttribute]
  [PathCompleter]
)
$TypeAcceleratorsClass = [System.Management.Automation.PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [System.Management.Automation.InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
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
