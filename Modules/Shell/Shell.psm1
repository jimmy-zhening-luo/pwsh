function Format-Path {
  [OutputType([string])]
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
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [System.IO.Path]::GetRelativePath($Path, $Location) -match '^[.\\]*$'
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

  $Path = Format-Path -Path $Path -Leading
  $Location = Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
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
      $Location = [System.IO.Path]::GetPathRoot($Path)
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
    throw "Invalid path '$Path': " + ($PSBoundParameters | ConvertTo-Json)
  }

  $Path = Format-Path -Path $Path -Leading
  $Location = Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
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
      Path      = $Local:root
      Directory = $this.Type -eq 'Directory'
      File      = $this.Type -eq 'File'
    }
    $word = $wordToComplete -replace '[\\\/]+', '\' -replace '^\\', ''
    $leaves = @()
    $subpath = ''
    $resultList = [System.Collections.Generic.List[System.Management.Automation.CompletionResult]]::new()

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
    $directories = $directories |
      Select-Object -ExpandProperty Name
    $files = $files |
      Select-Object -ExpandProperty Name

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

    if ($separator -ne '\') {
      $directories = $directories -replace '[\\]+', '/'
      $files = $files -replace '[\\]+', '/'
    }

    foreach ($item in $directories) {
      $string = [System.Management.Automation.Language.CodeGeneration]::EscapeSingleQuotedStringContent($item)
      $completion = $string -match '\s' ? "'" + $string + "'" : $string

      $resultList.Add(
        [System.Management.Automation.CompletionResult]::new(
          $completion
        )
      )
    }
    foreach ($item in $files) {
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
