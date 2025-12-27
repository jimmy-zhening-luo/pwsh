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
    [string[]]$Uri
  )

  begin {
    $Browser = "$env:ProgramFiles\Google\Chrome\Application\chrome.exe"
  }

  process {
    if ($PSCmdlet.ParameterSetName -eq 'Uri') {
      foreach ($link in $Uri) {
        if ($link) {
          if (-not $env:SSH_CLIENT) {
            Start-Process -FilePath $Browser -ArgumentList $link
          }
          else {
            Write-Information -MessageData "$link" -InformationAction Continue
          }
        }
      }
    }
  }

  end {
    if ($PSCmdlet.ParameterSetName -eq 'Path') {
      if (-not $env:SSH_CLIENT) {
        Start-Process -FilePath $Browser -ArgumentList (
          $Path ? (
            Test-Path $Path
          ) ? (
            Resolve-Path $Path
          ).Path : [string]([uri]$Path) : $PWD.Path
        )
      }
    }
  }
}

New-Alias go Open-Url
