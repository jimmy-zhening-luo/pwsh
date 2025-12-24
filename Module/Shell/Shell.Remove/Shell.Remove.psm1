using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Remove-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function Remove-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'Path',
    SupportsShouldProcess,
    SupportsTransactions,
    ConfirmImpact = 'Medium'
  )]
  [OutputType([void])]
  param(

    [Parameter(
      ParameterSetName = 'Path',
      Mandatory,
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    [string[]]$Path,

    [Parameter(
      ParameterSetName = 'LiteralPath',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [Alias('PSPath', 'LP')]
    [string[]]$LiteralPath,

    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude
  )

  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Remove-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -Recurse -Force @PSBoundParameters }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: rm -rf $Path, $LiteralPath"
      )
    ) {
      $steppablePipeline.Begin($PSCmdlet)
    }
  }

  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: Remove-Item -Recurse -Force -Path [[$Path]] -LiteralPath [$LiteralPath] -- " + (ConvertTo-Json $PSBoundParameters -EnumsAsStrings -Depth 6)
      )
    ) {
      $steppablePipeline.Process($PSItem)
    }
  }

  end {
    if ($PSCmdlet.ShouldProcess('Transaction', 'Close')) {
      $steppablePipeline.End()
    }
  }
}
