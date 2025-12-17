using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Remove-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function Remove-Directory {

  [OutputType([void])]

  param(

    [PathCompletions(
      '.',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Hard = @{
    Recurse = $True
    Force   = $True
  }
  Remove-Item @Hard @PSBoundParameters
}
