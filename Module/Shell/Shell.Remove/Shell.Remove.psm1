using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Remove-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function Remove-Directory {

  [OutputType([void])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Hard = @{
    Recurse = $True
    Force   = $True
  }
  Remove-Item @Private:Hard @PSBoundParameters
}
