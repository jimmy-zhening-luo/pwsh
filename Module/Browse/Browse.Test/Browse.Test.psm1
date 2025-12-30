using namespace Completer

enum TestHostVerbosity {
  Quiet
  Detailed
}

enum TestHostWellKnownPort {
  HTTP = -4
  RDP
  SMB
  WINRM
}

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
      [TestHostWellKnownPort]
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
      [TestHostVerbosity]
    )]
    # The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet.
    [string]$InformationLevel,

    # Shorthand for InformationLevel Detailed
    [switch]$Detailed,

    [Parameter(DontShow)][switch]$z
  )

  begin {
    if ($Detailed) {
      $InformationLevel = [TestHostVerbosity]::Detailed
    }

    if ($InformationLevel -ne [TestHostVerbosity]::Detailed) {
      $InformationLevel = [TestHostVerbosity]::Quiet
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
              [string]::Empty { break }
              {
                $null -ne [TestHostWellKnownPort]::$CommonTCPPort
              } {
                $Destination.CommonTCPPort = [TestHostWellKnownPort]::$CommonTCPPort
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

<#
.SYNOPSIS
Determine if an URL is reachable.

.DESCRIPTION
This function checks if an URL is reachable by sending a web request and checking the status code of the response.

It returns true if the URL returns a status code between 200 to 300, otherwise false.

The function times out if it receives no response after five (5) (lol) seconds, returning false.

.COMPONENT
Browse.Test

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/invoke-webrequest

.LINK
Invoke-WebRequest
#>
function Test-Url {
  [CmdletBinding()]
  [Alias('tu')]
  [OutputType([uri])]
  param(

    [Parameter(
      Mandatory,
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName,
      ValueFromRemainingArguments
    )]
    [AllowEmptyCollection()]
    [AllowEmptyString()]
    [AllowNull()]
    # The URL to test. If the URL has no scheme, it defaults to 'http'.
    [uri[]]$Uri,

    [Parameter(DontShow)][switch]$z
  )

  begin {
    $Request = @{
      Method                       = 'HEAD'
      PreserveHttpMethodOnRedirect = $True
      DisableKeepAlive             = $True
      ConnectionTimeoutSeconds     = 5
      MaximumRetryCount            = 0
      ErrorAction                  = 'Stop'
    }
  }

  process {
    foreach ($link in $Uri) {
      if ($link) {
        try {
          [int]$Status = Invoke-WebRequest -Uri $link @Request |
            Select-Object -ExpandProperty StatusCode
        }
        catch [Microsoft.PowerShell.Commands.HttpResponseException] {
          $Status = $PSItem.Exception.Response.StatusCode.value__
        }
        catch [System.Net.Http.HttpRequestException] {
          [int]$Status = -1
        }
        catch {
          throw 'Test-Url: Unhandled exception: ' + $PSItem.Exception
        }

        if ($Status -as [int] -and $Status -ge 200 -and $Status -lt 300) {
          Write-Output ([uri]$link)
        }
      }
    }
  }
}
