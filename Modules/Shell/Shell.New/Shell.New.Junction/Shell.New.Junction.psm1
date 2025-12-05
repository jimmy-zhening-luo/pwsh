New-Alias mj Shell\New-Junction
<#
.FORWARDHELPTARGETNAME New-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function New-Junction {
  [CmdletBinding(
    DefaultParameterSetName = 'pathSet',
    SupportsShouldProcess,
    SupportsTransactions,
    ConfirmImpact = 'Medium'
  )]
  [OutputType([System.IO.DirectoryInfo])]
  param(
    [Parameter(
      ParameterSetName = 'pathSet',
      Mandatory,
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    [string[]]$Path,
    [Parameter(
      Mandatory,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias('Target')]
    [PathCompletions('.')]
    [Object]$Value
  )
  begin {
    $type = @{
      ItemType = 'Directory'
      Force    = $True
    }
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd @type @PSBoundParameters @args }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: Create junction [$Path] with target [$Value]"
      )
    ) {
      $steppablePipeline.Begin($PSCmdlet)
    }
  }
  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: New-Item -Force -ItemType Junction -Path [$Path] -Value [$Value] -- $args"
      )
    ) {
      $steppablePipeline.Process($_)
    }
  }
  end {
    if ($PSCmdlet.ShouldProcess('Transaction', 'Close')) {
      $steppablePipeline.End()
    }
  }
}
