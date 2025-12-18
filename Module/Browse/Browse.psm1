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
    DefaultParameterSetName = 'Path'
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
    [hashtable]$Private:Browser = @{
      FilePath = 'C:\Program Files\Google\Chrome\Application\chrome.exe'
    }
  }

  process {
    if ($PSCmdlet.ParameterSetName -eq 'Uri') {
      foreach ($Private:link in $Uri) {
        if ($link) {
          if ($Interactive) {
            Start-Process @Browser -ArgumentList $link
          }
          else {
            Write-Information $link
          }
        }
      }
    }
  }

  end {
    if ($PSCmdlet.ParameterSetName -eq 'Path') {
      [string]$Private:Target = $Path ? (
        Test-Path @PSBoundParameters
      ) ? (
        Resolve-Path @PSBoundParameters
      ).Path : [uri]$Path : $PWD.Path

      if ($Interactive) {
        Start-Process @Browser -ArgumentList $Target
      }
    }
  }
}

New-Alias go Open-Url
