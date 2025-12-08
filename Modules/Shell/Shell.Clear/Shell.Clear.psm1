New-Alias cl Shell\Clear-Line
function Clear-Line {
  [OutputType([void])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($Path -or $args) {
    [void](Clear-Content @PSBoundParameters @args)
  }
  else {
    [void](Clear-Host)
  }
}

function Remove-Directory {
  [OutputType([void])]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('.')]
    [string]$Path
  )

  $Hard = @{
    Recurse = $True
    Force   = $True
  }
  [void](Remove-Item @Hard @PSBoundParameters)
}
