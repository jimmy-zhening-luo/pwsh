New-Alias mj New-Junction
<#
.FORWARDHELPTARGETNAME New-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function New-Junction {
  [CmdletBinding(
    SupportsShouldProcess,
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo])]
  param(
    [Parameter(
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [string[]]${Path},
    [Parameter(
      Mandatory,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias("Target")]
    [Object]${Value}
  )
  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -Force -ItemType Junction @PSBoundParameters }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: Create $($Path.Count) junction(s) [[$Path]]"
      )
    ) {
      $steppablePipeline.Begin($PSCmdlet)
    }
  }
  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: New-Item -Force -ItemType Junction -Path [$Path]"
      )
    ) {
      $steppablePipeline.Process($_)
    }
  }
  end {
    if (
      $PSCmdlet.ShouldProcess(
        "Transaction",
        "Close"
      )
    ) {
      $steppablePipeline.End()
    }
  }
}
