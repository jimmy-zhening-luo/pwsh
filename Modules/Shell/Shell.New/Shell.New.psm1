#Requires -Modules Microsoft.PowerShell.Management, Microsoft.PowerShell.Utility

using namespace System.IO
using namespace System.Management.Automation

Microsoft.PowerShell.Utility\New-Alias touch Microsoft.PowerShell.Management\New-Item

Microsoft.PowerShell.Utility\New-Alias mk Shell\New-Directory
<#
.FORWARDHELPTARGETNAME Microsoft.PowerShell.Management\New-Item
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
    [PathCompletions('.')]
    [Object]$Value,
    [switch]$Force,
    [Parameter(
      ValueFromPipelineByPropertyName
    )]
    [System.Management.Automation.PSCredential]$Credential
  )
  begin {
    $DirectoryType = @{
      ItemType = 'Directory'
    }
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Microsoft.PowerShell.Management\New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd @DirectoryType @PSBoundParameters }
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
        "> Step: Microsoft.PowerShell.Management\New-Item -ItemType Directory -Path [[$Path]] -Name [$Name] -- " + ($PSBoundParameters | Microsoft.PowerShell.Utility\ConvertTo-Json -EnumsAsStrings)
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

Microsoft.PowerShell.Utility\New-Alias mj Shell\New-Junction
<#
.FORWARDHELPTARGETNAME Microsoft.PowerShell.Management\New-Item
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
    $JunctionType = @{
      ItemType = 'Junction'
      Force    = $True
    }
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Microsoft.PowerShell.Management\New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd @PSBoundParameters @JunctionType }
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
        "> Step: Microsoft.PowerShell.Management\New-Item -Force -ItemType Junction -Path [$Path] -Value [$Value]"
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
