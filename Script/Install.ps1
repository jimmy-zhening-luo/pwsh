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
    Mandatory,
    Position = 1
  )]
  [ValidateNotNullOrEmpty()]
  [string]$ModuleRoot,

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

#region Modules
foreach ($Private:Module in $Modules) {
  [hashtable]$Private:ModuleDistro = @{
    Destination = "$ModuleRoot\$Module"
    Force       = $True
  }
  [hashtable]$Private:ModuleAssembly = @{
    Path = "$($ModuleDistro.Destination)\$Module.dll"
  }

  [hashtable]$Private:Built = @{
    Path = "$ClassRoot\$Module\bin\Release\netstandard2.0\$Module.dll"
  }
  if (Test-Path @Built) {
    if (
      -not (
        Test-Path @ModuleAssembly -PathType Leaf
      ) -or (
        Get-FileHash @ModuleAssembly
      ).Hash -ne (
        Get-FileHash @Built
      ).Hash
    ) {
      try {
        Copy-Item @Built @ModuleDistro
      }
      catch {
        [hashtable]$Private:Exception = @{
          Message   = "Failed to install $Module to $($ModuleAssembly.Path)."
          Exception = $PSItem.Exception
        }
        Write-Error @Exception
      }
    }
  }
  else {
    Write-Warning -Message "$Module has not been built, skipping."
  }
}
#endregion

#region Types
foreach ($Private:Type in $Types) {
  [hashtable]$Private:TypeDistro = @{
    Destination = $ClassRoot
    Force       = $True
  }
  [hashtable]$Private:TypeAssembly = @{
    Path = "$($TypeDistro.Destination)\$Type.dll"
  }

  [hashtable]$Private:Built = @{
    Path = "$ClassRoot\$Type\bin\Release\netstandard2.0\$Type.dll"
  }
  if (Test-Path @Built) {
    if (
      -not (
        Test-Path @TypeAssembly -PathType Leaf
      ) -or (
        Get-FileHash @TypeAssembly
      ).Hash -ne (
        Get-FileHash @Built
      ).Hash
    ) {
      try {
        Copy-Item @Built @TypeDistro
      }
      catch {
        [hashtable]$Private:Exception = @{
          Message   = "Failed to install $Type to $($TypeAssembly.Path)."
          Exception = $PSItem.Exception
        }
        Write-Error @Exception
      }
    }
  }
  else {
    Write-Warning -Message "$Type has not been built, skipping."
  }
}
#endregion

#region Accelerate
foreach ($Private:Type in $Types) {
  [hashtable]$Private:Assembly = @{
    Path = "$ClassRoot\$Type.dll"
  }
  if (Test-Path @Assembly -PathType Leaf) {
    Add-Type @Assembly
  }
}
#endregion
