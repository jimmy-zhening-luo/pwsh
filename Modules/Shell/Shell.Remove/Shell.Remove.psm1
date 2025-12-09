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
  Remove-Item @Hard @PSBoundParameters
}
