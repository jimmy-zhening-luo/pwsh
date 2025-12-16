using namespace System.Collections.Generic

[CmdletBinding()]
param (
  [Parameter(
    Mandatory,
    Position = 0
  )]
  [ValidateNotNullOrEmpty()]
  [string]$ClassRoot,
  [Parameter(
    Mandatory
  )]
  [AllowEmptyCollection()]
  [ValidateNotNull()]
  [string[]]$Types,
  [Parameter(
    Mandatory
  )]
  [AllowEmptyCollection()]
  [ValidateNotNull()]
  [string[]]$Modules
)

#region Install
$Private:FoundTypes = [List[string]]::new()

foreach ($Private:Type in $Types) {
  if (-not [string]::IsNullOrWhiteSpace($Type)) {
    if (Test-Path -Path "$ClassRoot\$Type" -PathType Container) {
      $Private:FoundTypes.Add($Type)
    }
  }
}

[hashtable]$Private:InstallType = @{
  Destination = $ClassRoot
  Force       = $true
}
foreach ($Private:Type in $FoundTypes) {
  [hashtable]$Private:Built = @{
    Path = "$ClassRoot\$Type\bin\Release\netstandard2.0\$Type.dll"
  }
  if (Test-Path @Built) {
    [hashtable]$Private:Installation = @{
      Path = "$($InstallType.Destination)\$Type.dll"
    }

    if (
      -not (
        Test-Path @Installation -PathType Leaf
      ) -or (
        Get-FileHash @Installation
      ).Hash -ne (
        Get-FileHash @Built
      ).Hash
    ) {
      try {
        Copy-Item @Built @InstallType
      }
      catch {
        $Message = "Failed to install $Type to $($InstallType.Destination)."
        Write-Error -Message $Message -Exception $_.Exception
      }
    }
  }
  else {
    $Message = "Built assembly for $Type not found at $($Built.Path)."
    Write-Warning -Message $Message
  }
}
#endregion

#region Type
foreach ($Private:Type in $Types) {
  [hashtable]$Private:Assembly = @{
    Path = "$ClassRoot\$Type.dll"
  }
  if (Test-Path @Assembly -PathType Leaf) {
    Add-Type @Assembly
  }
}
#endregion
