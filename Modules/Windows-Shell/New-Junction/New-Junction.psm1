New-Alias mj New-Junction
function New-Junction {
  [CmdletBinding(
    SupportsShouldProcess,
    SupportsTransactions
  )]
  param(
    [Parameter(
      ParameterSetName = 'pathSet',
      Mandatory,
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    [string[]]${Path},
    [Parameter(
      Mandatory,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias('Target')]
    [PathCompletions('.')]
    [Object]${Value}
  )
  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -Force -ItemType Junction @PSBoundParameters @args }
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
    if (
      $PSCmdlet.ShouldProcess(
        'Transaction',
        'Close'
      )
    ) {
      $steppablePipeline.End()
    }
  }
}
