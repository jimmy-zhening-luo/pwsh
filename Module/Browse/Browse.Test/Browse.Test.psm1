using namespace Completer

<#
.SYNOPSIS
Determine if a host is reachable.

.DESCRIPTION
This function checks if a host is reachable by testing the network connection to the specified hostname or IP address, optionally on a specified port (number or well-known TCP service).

.COMPONENT
Browse.Test

.LINK
https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection

.LINK
Test-NetConnection
#>
function Test-Host {
  [CmdletBinding(
    DefaultParameterSetName = 'CommonTCPPort'
  )]
  [Alias('tn')]
  [OutputType([System.Object])]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias('ComputerName', 'RemoteAddress', 'cn', 'HostName', 'IpAddress')]
    # The hostname or IP address of the target host.
    [string[]]$Name,

    [Parameter(
      ParameterSetName = 'CommonTCPPort',
      Position = 1
    )]
    [Alias('TCP')]
    [EnumCompletions(
      [Browse.TestHostWellKnownPort]
    )]
    # Specifies the common service TCP port number.
    [string]$CommonTCPPort,

    [Parameter(
      ParameterSetName = 'RemotePort',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [ValidateRange('Positive')]
    # The port number to test on the target host.
    [ushort]$Port,

    [EnumCompletions(
      [Browse.TestHostVerbosity]
    )]
    # The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet.
    [string]$InformationLevel,

    # Shorthand for InformationLevel Detailed
    [switch]$Detailed
  )

  begin {
    if ($Detailed) {
      $InformationLevel = [Browse.TestHostVerbosity]::Detailed
    }

    if ($InformationLevel -ne [Browse.TestHostVerbosity]::Detailed) {
      $InformationLevel = [Browse.TestHostVerbosity]::Quiet
    }
  }

  process {
    foreach ($computerName in $Name) {
      if ($computerName) {
        $Destination = @{
          ComputerName = $computerName
        }
        switch ($PSCmdlet.ParameterSetName) {
          RemotePort {
            $Destination.Port = $Port
            break
          }
          CommonTCPPort {
            switch ($CommonTCPPort) {
              '' { break }
              {
                $null -ne [Browse.TestHostWellKnownPort]::$CommonTCPPort
              } {
                $Destination.CommonTCPPort = [Browse.TestHostWellKnownPort]::$CommonTCPPort
                break
              }
              {
                $CommonTCPPort -as [ushort]
              } {
                $Destination.Port = [ushort]$CommonTCPPort
              }
            }
          }
        }

        Test-NetConnection @Destination -InformationLevel $InformationLevel
      }
    }
  }

  end {
    if (-not $Name) {
      $DumbassPSLinterWorkaroundLMFAO = 'google.com'

      return Test-NetConnection -ComputerName $DumbassPSLinterWorkaroundLMFAO -InformationLevel $InformationLevel
    }
  }
}
