using namespace System.Collections.Generic

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
      [string]$Private:Target = $Path ? (Test-Path @PSBoundParameters) ? (Resolve-Path @PSBoundParameters).Path : [uri]$Path : $PWD.Path

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

New-Alias go Open-Url
