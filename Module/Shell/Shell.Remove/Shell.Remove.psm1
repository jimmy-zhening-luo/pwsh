using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Remove-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function Remove-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    [string]$Path
  )

  Remove-Item -Path $Path -Recurse -Force
}
