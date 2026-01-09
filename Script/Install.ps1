using namespace System.Collections.Generic

& {
  $ROOT = Split-Path $PSScriptRoot
  $Types = @(
    'Completer'
    'Context'
  )

  function Test-PSAssembly {
    [CmdletBinding()]
    [OutputType([bool])]
    param(
      [Parameter(
        Mandatory,
        Position = 0
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string[]]$Source,

      [Parameter(
        Mandatory,
        Position = 1
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Destination
    )

    end {
      return (
        -not (
          Test-Path $Destination -PathType Leaf
        ) -or (
          Get-FileHash -Path $Destination -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -Path $Source -Algorithm MD5
        ).Hash
      )
    }
  }

  function Install-PSAssembly {
    [CmdletBinding()]
    [OutputType([void])]
    param(
      [Parameter(
        Mandatory,
        ValueFromPipeline
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string[]]$Project,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Class,

      [switch]$Module
    )

    process {
      $BuildOutput = "$ROOT\Class\$Class\" + (
        $Module ? '' : "$Project\"
      ) + "bin\Release\net9.0\$Project.dll"

      if (-not (Test-Path $BuildOutput -PathType Leaf)) {
        Write-Warning -Message "Project '$Class\$Project' is not built, skipping."
      }

      $InstallPath = "$ROOT\$Class" + (
        $Module ? 'Module' : ''
      )
      $InstalledAssembly = "$InstallPath\$Project.dll"

      if (Test-PSAssembly $BuildOutput $InstalledAssembly) {
        Copy-Item -Path $BuildOutput -Destination $InstallPath -Force -ErrorAction Continue
      }
    }
  }

  "Module" |
    Install-PSAssembly -Class Module -Module
  $Types |
    Install-PSAssembly -Class Type

  $Types |
    Where-Object {
      Test-Path $Root\Type\$PSItem.dll -PathType Leaf
    } |
    ForEach-Object {
      Add-Type -Path $Root\Type\$PSItem.dll
    }
}
