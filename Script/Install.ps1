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
  [hashtable[]]$Types,

  [Parameter(
    Mandatory
  )]
  [AllowEmptyCollection()]
  [ValidateNotNull()]
  [hashtable[]]$Modules

)

#region Modules
foreach ($Private:Module in $Modules) {
  [string]$Private:Name = $Module.Name
  [string]$Private:Runtime = $Module.Runtime

  [hashtable]$Private:ModuleDistro = @{
    Destination = "$ModuleRoot\$Name"
    Force       = $True
  }
  [hashtable]$Private:ModuleAssembly = @{
    Path = "$($ModuleDistro.Destination)\$Name.dll"
  }

  [hashtable]$Private:Built = @{
    Path = "$ClassRoot\$Name\bin\Release\$Runtime\$Name.dll"
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
          Message   = "Failed to install $Name to $($ModuleAssembly.Path)."
          Exception = $PSItem.Exception
        }
        Write-Error @Exception
      }
    }
  }
  else {
    Write-Warning -Message "$Name has not been built, skipping."
  }
}
#endregion

#region Types
foreach ($Private:Type in $Types) {
  [string]$Private:Name = $Type.Name
  [string]$Private:Runtime = $Type.Runtime

  [hashtable]$Private:TypeDistro = @{
    Destination = $ClassRoot
    Force       = $True
  }
  [hashtable]$Private:TypeAssembly = @{
    Path = "$($TypeDistro.Destination)\$Name.dll"
  }

  [hashtable]$Private:Built = @{
    Path = "$ClassRoot\$Name\bin\Release\$Runtime\$Name.dll"
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
          Message   = "Failed to install $Name to $($TypeAssembly.Path)."
          Exception = $PSItem.Exception
        }
        Write-Error @Exception
      }
    }
  }
  else {
    Write-Warning -Message "$Name has not been built, skipping."
  }
}
#endregion

#region Accelerate
foreach ($Private:Type in $Types) {
  [hashtable]$Private:Assembly = @{
    Path = "$ClassRoot\$($Type.Name).dll"
  }
  if (Test-Path @Assembly -PathType Leaf) {
    Add-Type @Assembly
  }
}
#endregion
