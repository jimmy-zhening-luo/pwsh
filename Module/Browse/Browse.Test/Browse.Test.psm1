using namespace System.Collections.Generic

enum TestHostVerbosity {
  Quiet
  Detailed
}

enum TestHostWellknownPort {
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
Browse

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
    [string]$Name,

    [Parameter(
      ParameterSetName = 'CommonTCPPort',
      Position = 1
    )]
    [Alias('TCP')]
    [Completions(
      {
        return [TestHostWellknownPort].GetEnumNames()
      }
    )]
    [string]$CommonTCPPort,

    [Parameter(
      ParameterSetName = 'RemotePort',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [ValidateRange(1, 65535)]
    # The port number to test on the target host.
    [UInt16]$Port,

    [Completions(
      {
        return [TestHostVerbosity].GetEnumNames()
      }
    )]
    # The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet.
    [string]$InformationLevel,

    # Shorthand for InformationLevel Detailed
    [switch]$Detailed

  )

  begin {
    $Private:Results = [List[System.Object]]::new()

    if ($Detailed) {
      $InformationLevel = [TestHostVerbosity]::Detailed
    }
    [hashtable]$Private:Verbosity = @{
      InformationLevel = $InformationLevel -eq [TestHostVerbosity]::Detailed ? [TestHostVerbosity]::Detailed : [TestHostVerbosity]::Quiet
    }
  }

  process {
    if ($Name) {
      [hashtable]$Private:Connection = @{
        ComputerName = $Name
      }
      switch ($PSCmdlet.ParameterSetName) {
        RemotePort {
          $Connection.Port = $Port
        }
        CommonTCPPort {
          if ($CommonTCPPort) {
            if ([TestHostWellknownPort]::$CommonTCPPort) {
              $Connection.CommonTCPPort = [TestHostWellknownPort]::$CommonTCPPort
            }
            elseif (
              $CommonTCPPort -match [regex]'^(?>\d{1,5})$' -and $CommonTCPPort -as [UInt16]
            ) {
              $Connection.Port = [UInt16]$CommonTCPPort
            }
          }
        }
      }

      $Private:Result = Test-NetConnection @Connection @Verbosity

      if ($Result) {
        $Results.Add($Result)
      }
    }
  }

  end {
    if ($Results.Count -eq 0) {
      [hashtable]$Private:Connection = @{
        ComputerName = 'google.com'
      }
      $Private:Result = Test-NetConnection @Connection @Verbosity

      if ($Result) {
        $Results.Add($Result)
      }
    }

    return $Results.ToArray()
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
Browse

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/invoke-webrequest

.LINK
Invoke-WebRequest
#>
function Test-Url {

  [CmdletBinding()]

  [OutputType([bool])]

  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [AllowNull()]
    [AllowEmptyString()]
    # The URL to test. If the URL has no scheme, it defaults to 'http'.
    [uri]$Uri

  )

  if (-not $Uri) {
    return $False
  }

  [hashtable]$Private:Request = @{
    Method                       = 'HEAD'
    PreserveHttpMethodOnRedirect = $True
    DisableKeepAlive             = $True
    ConnectionTimeoutSeconds     = 5
    MaximumRetryCount            = 0
    ErrorAction                  = 'Stop'
  }
  try {
    [int]$Private:Status = Invoke-WebRequest @PSBoundParameters @Request |
      Select-Object -ExpandProperty StatusCode
  }
  catch {
    $Private:Status = $PSItem.Exception.Response.StatusCode.value__
  }

  return $Status -ge 200 -and $Status -lt 300
}

New-Alias tn Test-Host
New-Alias tu Test-Url
