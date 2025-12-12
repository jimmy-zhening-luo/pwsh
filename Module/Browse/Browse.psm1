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

New-Alias tn Test-Host
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
    [GenericCompletions('HTTP,RDP,SMB,WINRM')]
    [string]$CommonTCPPort,

    [Parameter(
      ParameterSetName = 'RemotePort',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [ValidateRange(1, 65535)]
    # The port number to test on the target host.
    [UInt16]$Port,

    [GenericCompletions('Quiet,Detailed')]
    # The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet.
    [TestHostVerbosity]$InformationLevel = [TestHostVerbosity]::Quiet,

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
            if (
              [System.Enum]::IsDefined(
                [TestHostWellknownPort],
                $CommonTCPPort
              )
            ) {
              $Connection.CommonTCPPort = [TestHostWellknownPort].GetEnumName($CommonTCPPort)
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

New-Alias tu Test-Url
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

New-Alias go Open-Url
<#
.SYNOPSIS
Open a file path or URL in Google Chrome.

.DESCRIPTION
This function opens the specified file path or URL in Google Chrome. If a file path is provided, it resolves the path before opening it. If the file path cannot be resolved to the filesystem, it casts the path to an URL, throwing an error if the cast is unsuccessful. If an URL is provided, it opens the URI directly.

.COMPONENT
Browse

.LINK
https://www.chromium.org/developers/how-tos/run-chromium-with-flags/
#>
function Open-Url {

  [CmdletBinding(
    DefaultParameterSetName = 'Path',
    SupportsShouldProcess
  )]

  [OutputType([void])]

  param(

    [Parameter(
      ParameterSetName = 'Path',
      Position = 0
    )]
    [AllowEmptyString()]
    # The file path or URL to open. Defaults to the current directory.
    [string]$Path,

    [Parameter(
      ParameterSetName = 'Uri',
      Mandatory,
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyCollection()]
    # The URL(s) to open.
    [uri[]]$Uri

  )

  begin {
    [bool]$Private:Interactive = -not $env:SSH_CLIENT
  }

  process {
    if ($Uri) {
      if (
        $PSCmdlet.ShouldProcess(
          $Uri,
          "[ui=$Interactive] $($Interactive ? 'Open' : 'Print') URI"
        )
      ) {
        if ($Interactive) {
          [hashtable]$Private:Browser = @{
            FilePath     = 'C:\Program Files\Google\Chrome\Application\chrome.exe'
            ArgumentList = $Uri
          }
          Start-Process @Browser
        }
      }
    }
  }

  end {
    if ($Path) {
      [string]$Private:Target = $Path ? (Test-Path @PSBoundParameters) ? (Resolve-Path @PSBoundParameters) : [uri]$Path : $PWD

      if (
        $PSCmdlet.ShouldProcess(
          $Target,
          "[ui=$Interactive] $($Interactive ? 'Open' : 'Print') path"
        )
      ) {
        if ($Interactive) {
          [hashtable]$Private:Browser = @{
            FilePath     = 'C:\Program Files\Google\Chrome\Application\chrome.exe'
            ArgumentList = $Target
          }
          Start-Process @Browser
        }
      }
    }
  }
}
