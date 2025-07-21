New-Alias -Option ReadOnly mj New-Junction
<#
.FORWARDHELPTARGETNAME New-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function New-Junction {
  [CmdletBinding(SupportsShouldProcess)]
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
    $steppablePipeline.Begin($PSCmdlet)
  }

  process {
    if ($PSCmdlet.ShouldProcess($Path, "Create junction")) {
      $steppablePipeline.Process($_)
    }
  }

  end {
    $steppablePipeline.End()
  }
}
