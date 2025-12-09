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
    $TrimmedPath = $TrimmedPath -replace '^(?>\.(?>\\+))', ''
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

  [System.IO.Path]::GetRelativePath($Path, $Location) -match '^(?>[.\\]*)$'
}

function Merge-RelativePath {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [System.IO.Path]::GetRelativePath($Location, $Path)
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

  $Path = Shell\Format-Path -Path $Path -LeadingRelative
  $Location = Shell\Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Relative = @{
        Path     = $Path
        Location = $Location
      }
      if (Shell\Trace-RelativePath @Relative) {
        $Path = Shell\Merge-RelativePath @Relative
      }
      else {
        return $False
      }
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^(~(?>\\*))', ''

    if ($Location) {
      $Relative = @{
        Path     = Microsoft.PowerShell.Management\Join-Path $HOME $Path
        Location = $Location
      }
      if (Shell\Trace-RelativePath @Relative) {
        $Path = Shell\Merge-RelativePath @Relative
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
    $Location = (Microsoft.PowerShell.Management\Get-Location).Path
  }

  $Container = @{
    Path     = $Location
    PathType = 'Container'
  }
  if (-not (Microsoft.PowerShell.Management\Test-Path @Container)) {
    return $False
  }

  $FullLocation = (Microsoft.PowerShell.Management\Resolve-Path -Path $Location).Path
  $FullPath = Microsoft.PowerShell.Management\Join-Path $FullLocation $Path
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
    (Microsoft.PowerShell.Management\Test-Path @Item -IsValid) -and -not (Microsoft.PowerShell.Management\Test-Path @Item)
  }
  else {
    Microsoft.PowerShell.Management\Test-Path @Item
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
    throw "Invalid path '$Path': " + ($PSBoundParameters | Microsoft.PowerShell.Utility\ConvertTo-Json -EnumsAsStrings)
  }

  $Path = Shell\Format-Path -Path $Path -LeadingRelative
  $Location = Shell\Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Shell\Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^(?>~(?>\\*))', ''

    if ($Location) {
      $Path = Shell\Merge-RelativePath -Path (
        Microsoft.PowerShell.Management\Join-Path $HOME $Path
      ) -Location $Location
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = (Microsoft.PowerShell.Management\Get-Location).Path
  }

  $FullLocation = (Microsoft.PowerShell.Management\Resolve-Path -Path $Location).Path
  $FullPath = Microsoft.PowerShell.Management\Join-Path $FullLocation $Path

  $New ? $FullPath : (
    Microsoft.PowerShell.Management\Resolve-Path -Path $FullPath -Force
  ).Path
}

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
    if (-not $root -or -not (Microsoft.PowerShell.Management\Test-Path -Path $root -PathType Container)) {
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

    $Local:root = Microsoft.PowerShell.Management\Resolve-Path -Path $this.Root
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

      $currentDirectoryText = Microsoft.PowerShell.Management\Split-Path $currentPathText
      $fragment = Microsoft.PowerShell.Management\Split-Path $currentPathText -Leaf

      if ($fragment -eq '*') {
        $fragment = ''
      }

      $path = Microsoft.PowerShell.Management\Join-Path $Local:root $currentDirectoryText

      if (Microsoft.PowerShell.Management\Test-Path -Path $path -PathType Container) {
        $query.Path = $path
        $query['Filter'] = "$fragment*"
        $leaves = Microsoft.PowerShell.Management\Get-ChildItem @query
      }
    }

    if (-not $query.Path) {
      $query.Path = $Local:root
    }

    $leaves = @()
    $leaves += Microsoft.PowerShell.Management\Get-ChildItem @query
    $directories, $files = $leaves.Where(
      { $PSItem.PSIsContainer },
      'Split'
    )
    $directories = $directories |
      Microsoft.PowerShell.Utility\Select-Object -ExpandProperty Name
    $files = $files |
      Microsoft.PowerShell.Utility\Select-Object -ExpandProperty Name

    if ($currentDirectoryText -and -not $this.Flat) {
      $directories += ''
    }

    if ($currentDirectoryText) {
      $directories = $directories |
        ForEach-Object { Microsoft.PowerShell.Management\Join-Path $currentDirectoryText $PSItem }
      $files = $files |
        ForEach-Object { Microsoft.PowerShell.Management\Join-Path $currentDirectoryText $PSItem }
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
