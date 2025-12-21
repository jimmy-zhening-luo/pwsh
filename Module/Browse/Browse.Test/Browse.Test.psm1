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
  [OutputType([System.Object[]])]
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
    [StaticCompletions(
      'http,rdp,smb,winrm',
      $null, $null
    )]
    [string]$CommonTCPPort,

    [Parameter(
      ParameterSetName = 'RemotePort',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [ValidateRange('Positive')]
    # The port number to test on the target host.
    [ushort]$Port,

    [StaticCompletions(
      'quiet,detailed',
      $null, $null
    )]
    # The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet.
    [string]$InformationLevel,

    # Shorthand for InformationLevel Detailed
    [switch]$Detailed,

    [Parameter(DontShow)][switch]$zNothing
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
    foreach ($Private:computerName in $Name) {
      if ($Private:computerName) {
        [hashtable]$Private:Destination = @{
          ComputerName = $Private:computerName
        }
        switch ($PSCmdlet.ParameterSetName) {
          RemotePort {
            $Private:Destination.Port = $Port
          }
          CommonTCPPort {
            if ($CommonTCPPort) {
              if ([TestHostWellKnownPort]::$CommonTCPPort) {
                $Private:Destination.CommonTCPPort = [TestHostWellKnownPort]::$CommonTCPPort
              }
              elseif ($CommonTCPPort -as [ushort]) {
                $Private:Destination.Port = [ushort]$CommonTCPPort
              }
            }
          }
        }

        Test-NetConnection @Private:Destination -InformationLevel $Private:InformationLevel
      }
    }
  }

  end {
    if (-not $Name) {
      $Private:DumbassPSLinterWorkaroundLMFAO = 'google.com'

      return Test-NetConnection -ComputerName $Private:DumbassPSLinterWorkaroundLMFAO -InformationLevel $Private:InformationLevel
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
  [OutputType([uri[]])]
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

    [Parameter(DontShow)][switch]$zNothing
  )

  begin {
    [hashtable]$Private:Request = @{
      Method                       = 'HEAD'
      PreserveHttpMethodOnRedirect = $True
      DisableKeepAlive             = $True
      ConnectionTimeoutSeconds     = 5
      MaximumRetryCount            = 0
      ErrorAction                  = 'Stop'
    }
  }

  process {
    foreach ($Private:link in $Uri) {
      if ($Private:link) {
        try {
          [int]$Private:Status = Invoke-WebRequest -Uri $Private:link @Private:Request |
            Select-Object -ExpandProperty StatusCode
        }
        catch [Microsoft.PowerShell.Commands.HttpResponseException] {
          $Private:Status = $PSItem.Exception.Response.StatusCode.value__
        }
        catch [System.Net.Http.HttpRequestException] {
          [int]$Private:Status = -1
        }
        catch {
          throw 'Test-Url: Unhandled exception: ' + $PSItem.Exception
        }

        if ($Private:Status -as [int] -and $Private:Status -ge 200 -and $Private:Status -lt 300) {
          Write-Output -InputObject ([uri]$Private:link)
        }
      }
    }
  }
}

New-Alias tn Test-Host
New-Alias tu Test-Url
