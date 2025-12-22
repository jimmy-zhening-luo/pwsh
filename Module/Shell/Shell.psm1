using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path }
    )]
    [string]$Path
  )

  if ($Path -or $args) {
    Clear-Content -Path $Path @args
  }
  else {
    Clear-Host
  }
}

New-Alias cl Clear-Line
