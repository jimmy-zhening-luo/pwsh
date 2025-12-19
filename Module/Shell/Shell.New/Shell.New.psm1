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
      { return [string]$PWD.Path },
      $null, $null
    )]
    [System.Object]$Value,
    [switch]$Force,

    [Parameter(
      ValueFromPipelineByPropertyName
    )]
    [System.Management.Automation.PSCredential]$Credential,

    [Parameter(DontShow)][switch]$zNothing

  )

  begin {
    [hashtable]$Private:DirectoryType = @{
      ItemType = 'Directory'
    }
    $Private:wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $Private:scriptCmd = { & $Private:wrappedCmd @Private:DirectoryType @PSBoundParameters }
    $Private:steppablePipeline = $Private:scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: Create $($Path.Count) directory(s) [[$Path]]\$Name"
      )
    ) {
      $Private:steppablePipeline.Begin($PSCmdlet)
    }
  }

  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: New-Item -ItemType Directory -Path [[$Path]] -Name [$Name] -- " + ($PSBoundParameters | ConvertTo-Json -EnumsAsStrings -Depth 6)
      )
    ) {
      $Private:steppablePipeline.Process($PSItem)
    }
  }

  end {
    if ($PSCmdlet.ShouldProcess('Transaction', 'Close')) {
      $Private:steppablePipeline.End()
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
      { return [string]$PWD.Path },
      $null, $null
    )]
    [System.Object]$Value,

    [Parameter(DontShow)][switch]$zNothing

  )

  begin {
    [hashtable]$Private:JunctionType = @{
      ItemType = 'Junction'
      Force    = $True
    }
    $Private:wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $Private:scriptCmd = { & $Private:wrappedCmd @PSBoundParameters @Private:JunctionType }
    $Private:steppablePipeline = $Private:scriptCmd.GetSteppablePipeline()

    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "Open Transaction: Create junction [$Path] with target [$Value]"
      )
    ) {
      $Private:steppablePipeline.Begin($PSCmdlet)
    }
  }

  process {
    if (
      $PSCmdlet.ShouldProcess(
        $Value,
        "> Step: New-Item -Force -ItemType Junction -Path [$Path] -Value [$Value]"
      )
    ) {
      $Private:steppablePipeline.Process($PSItem)
    }
  }

  end {
    if ($PSCmdlet.ShouldProcess('Transaction', 'Close')) {
      $Private:steppablePipeline.End()
    }
  }
}

New-Alias touch New-Item

New-Alias mk New-Directory
New-Alias mj New-Junction
