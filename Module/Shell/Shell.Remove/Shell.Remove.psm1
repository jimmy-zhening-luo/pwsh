
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
