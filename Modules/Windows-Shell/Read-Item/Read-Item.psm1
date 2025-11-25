New-Alias p Read-Item
function Read-Item {
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [string]$Location
  )

  $Local:args = $args

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Local:args = , $Location + $Local:args
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Join-Path $Location $Path) : $Path

    if (-not (Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    $FullPath = @{
      Path = Resolve-Path -Path $Target |
        Select-Object -ExpandProperty Path
    }

    if (Test-Path @FullPath -PathType Container) {
      Get-ChildItem @FullPath @Local:args
    }
    else {
      Get-Content @FullPath @Local:args
    }
  }
  else {
    $Directory = @{
      Path = $Location ? (
        Resolve-Path -Path $Location | Select-Object -ExpandProperty Path
      ) : $PWD.Path
    }

    Get-ChildItem @Directory @Local:args
  }
}

New-Alias p. Read-Sibling
function Read-Sibling {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  $Location = @{
    Location = '..'
  }

  Read-Item @PSBoundParameters @Location @args
}

New-Alias p.. Read-Relative
function Read-Relative {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  $Location = @{
    Location = '..\..'
  }

  Read-Item @PSBoundParameters @Location @args
}

New-Alias p~ Read-Home
function Read-Home {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  $Location = @{
    Location = '~'
  }

  Read-Item @PSBoundParameters @Location @args
}

New-Alias pc Read-Code
function Read-Code {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  $Location = @{
    Location = '~\code'
  }

  Read-Item @PSBoundParameters @Location @args
}

New-Alias p\ Read-Drive
New-Alias p/ Read-Drive
function Read-Drive {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  $Location = @{
    Location = '\'
  }

  Read-Item @PSBoundParameters @Location @args
}
