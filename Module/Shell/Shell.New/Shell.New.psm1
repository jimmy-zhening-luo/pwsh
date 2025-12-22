using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME New-Item
.FORWARDHELPCATEGORY Cmdlet
#>
function New-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'pathSet',
    SupportsShouldProcess,
    SupportsTransactions,
    ConfirmImpact = 'Medium'
  )]
  [OutputType([System.IO.DirectoryInfo])]
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
    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    [System.Object]$Value,
    [switch]$Force,

    [Parameter(
      ValueFromPipelineByPropertyName
    )]
    [System.Management.Automation.PSCredential]$Credential
  )

  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd -ItemType Directory @PSBoundParameters }
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
        "> Step: New-Item -ItemType Directory -Path [[$Path]] -Name [$Name] -- " + (ConvertTo-Json -InputObject $PSBoundParameters -EnumsAsStrings -Depth 6)
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
    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    [System.Object]$Value
  )

  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd @PSBoundParameters -ItemType Junction -Force }
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
        "> Step: New-Item -Force -ItemType Junction -Path [$Path] -Value [$Value]"
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

New-Alias touch New-Item

New-Alias mk New-Directory
New-Alias mj New-Junction
