using namespace System.IO
using namespace System.Management.Automation

New-Alias mk Shell\New-Directory
<#
.FORWARDHELPTARGETNAME New-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function New-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'pathSet',
    SupportsShouldProcess,
    SupportsTransactions
  )]
  [OutputType([DirectoryInfo])]
  param(
    [Parameter(
      ParameterSetName = 'nameSet',
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    [Parameter(
      ParameterSetName = 'pathSet',
      Mandatory,
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    [string[]]$Path,
    [Parameter(
      ParameterSetName = 'nameSet',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [string]$Name,
    [Parameter(
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias('Target')]
    [PathCompletions('.')]
    [Object]$Value,
    [switch]$Force,
    [Parameter(
      ValueFromPipelineByPropertyName
    )]
    [PSCredential]$Credential
  )
  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -ItemType Directory @PSBoundParameters @args }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: Create $($Path.Count) directory(s) [[$Path]]\$Name"
      )
    ) {
      $steppablePipeline.Begin($PSCmdlet)
    }
  }
  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: New-Item -ItemType Directory -Path [[$Path]] -Name [$Name] -- @PSBoundParameters @args"
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
