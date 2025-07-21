New-Alias -Option ReadOnly mj New-Junction
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
    [System.String[]]${Path},
    [Parameter(
      Mandatory,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias("Target")]
    [System.Object]${Value},
    [Switch]${Force}
  )
  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -ItemType Junction @PSBoundParameters }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()

    if ($PSCmdlet.ShouldProcess($Value, "Open Transaction: Create $($Path.Count) junction(s) [[$Path]]")) {
      $steppablePipeline.Begin($PSCmdlet)
    }
  }
  process {
    if ($PSCmdlet.ShouldProcess($Value, "> Step: Create junction [$Path]")) {
      $steppablePipeline.Process($_)
    }
  }
  end {
    if ($PSCmdlet.ShouldProcess("Transaction", "Close")) {
      $steppablePipeline.End()
    }
  }
}
