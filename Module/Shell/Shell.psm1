using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {
  [CmdletBinding(
    DefaultParameterSetName = 'Path',
    SupportsTransactions
  )]
  [OutputType([void])]
  param(

    [Parameter(
      ParameterSetName = 'Path',
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path },
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [Parameter(
      ParameterSetName = 'LiteralPath',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [Alias('PSPath', 'LP')]
    [string[]]$LiteralPath,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('f')]
    [switch]$Force,

    [Parameter()]
    [string]$Stream
  )

  begin {
    if ($Path -or $LiteralPath) {
      $ProcessRecords = $True
      $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Clear-Content', [System.Management.Automation.CommandTypes]::Cmdlet)
      [scriptblock]$scriptCmd = { & $wrappedCmd @PSBoundParameters }
      $steppablePipeline = $scriptCmd.GetSteppablePipeline()
      $steppablePipeline.Begin($PSCmdlet)
    }
    else {
      $ProcessRecords = $False
    }
  }

  process {
    if ($ProcessRecords) {
      $steppablePipeline.Process($PSItem)
    }
  }

  end {
    if ($ProcessRecords) {
      $steppablePipeline.End()
    }
    else {
      Clear-Host
    }
  }
}

New-Alias cl Clear-Line
