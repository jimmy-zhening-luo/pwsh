New-Alias tn Browse\Test-Host
<#
.SYNOPSIS
Determine if a host is reachable.

.DESCRIPTION
This function checks if a host is reachable by testing the network connection to the specified hostname or IP address, optionally on a specified port (number or well-known TCP service).
#>
function Test-Host {
  [CmdletBinding(DefaultParameterSetName = 'CommonTCPPort')]
  [OutputType([Object[]])]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Alias('ComputerName', 'RemoteAddress', 'cn', 'HostName', 'IPAddress', 'ip')]
    # The hostname or IP address of the target host.
    [string]$Name,
    [Parameter(
      ParameterSetName = 'CommonTCPPort',
      Position = 1
    )]
    [GenericCompletions('HTTP,RDP,SMB,WINRM')]
    [Alias('TCP')]
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
    # The level of information to return.
    [string]$InformationLevel,
    [switch]$Quiet,
    [switch]$Detailed
  )
  begin {
    $InformationLevel = $Detailed -or $InformationLevel -eq 'Detailed' ? 'Detailed' : $Quiet -or $InformationLevel -eq 'Quiet' ? 'Quiet' : ''
    $Verbosity = $InformationLevel ? @{
      InformationLevel = $InformationLevel
    } : @{}
    $Results = @()
  }
  process {
    if ($Name) {
      $Connection = @{
        ComputerName = $Name
      }
      switch ($PSCmdlet.ParameterSetName) {
        RemotePort {
          $Connection.Port = $Port
        }
        default {
          if ($CommonTCPPort -in @('HTTP', 'RDP', 'SMB', 'WINRM')) {
            $Connection.CommonTCPPort = $CommonTCPPort.ToUpperInvariant()
          }
          elseif ($CommonTCPPort -match '^(?>\d{1,5})$' -and $CommonTCPPort -as [UInt16]) {
            $Connection.Port = [UInt16]$CommonTCPPort
          }
        }
      }

      Test-NetConnection @Connection @Verbosity
    }
  }
  end {
    if ($Results) {
      $Results
    }
    else {
      $Connection = @{
        ComputerName = [Uri]"http://google.com"
      }
      Test-NetConnection @Connection @Verbosity
    }
  }
}

New-Alias tu Browse\Test-Url
<#
.SYNOPSIS
Determine if an URL is reachable.
.DESCRIPTION
This function checks if an URL is reachable by sending a web request and checking the status code of the response.

It returns true if the URL returns a status code between 200 to 300, otherwise false.

The function times out if it receives no response after five (5) (lol) seconds, returning false.
#>
function Test-Url {
  [OutputType([bool])]
  param(
    # The URL to test. If the URL has no scheme, it defaults to 'http'.
    [Uri]$Uri
  )

  if (-not $Uri) {
    return $False
  }

  $Request = @{
    Uri                          = $Uri
    Method                       = 'HEAD'
    PreserveHttpMethodOnRedirect = $True
    DisableKeepAlive             = $True
    ConnectionTimeoutSeconds     = 5
    MaximumRetryCount            = 0
    ErrorAction                  = 'Stop'
  }
  try {
    $Status = Invoke-WebRequest @Request |
      Select-Object -ExpandProperty StatusCode
  }
  catch {
    $Status = $_.Exception.Response.StatusCode.value__
  }

  $Status -ge 200 -and $Status -lt 300
}

New-Alias go Browse\Open-Url
New-Alias open Browse\Open-Url
<#
.SYNOPSIS
Open a file path or URL in Google Chrome.
.DESCRIPTION
This function opens the specified file path or URL in Google Chrome. If a file path is provided, it resolves the path before opening it. If the file path cannot be resolved to the filesystem, it casts the path to an URL, throwing an error if the cast is unsuccessful. If an URL is provided, it opens the URI directly.
#>
function Open-Url {
  [CmdletBinding(DefaultParameterSetName = 'Path')]
  [OutputType([void])]
  param(
    [Parameter(
      ParameterSetName = 'Path',
      Position = 0
    )]
    [PathCompletions('.')]
    # The file path or URL to open. Defaults to the current directory.
    [string]$Path,
    [Parameter(
      ParameterSetName = 'Uri',
      Position = 0,
      Mandatory
    )]
    # The URL to open.
    [Uri]$Uri
  )

  switch ($PSCmdlet.ParameterSetName) {
    Uri {
      $Target = $Uri
    }
    default {
      $Target = $Path ? (Test-Path @PSBoundParameters) ? (Resolve-Path @PSBoundParameters) : [Uri]$Path : $PWD
    }
  }

  $Browser = @{
    FilePath     = 'C:\Program Files\Google\Chrome\Application\chrome.exe'
    ArgumentList = $Target
  }
  if (-not $env:SSH_CLIENT) {
    [void](Start-Process @Browser)
  }
}
