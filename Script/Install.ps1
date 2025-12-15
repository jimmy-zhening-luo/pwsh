[string]$Private:PROJECT_ROOT = Split-Path $PSScriptRoot
[string]$Private:CMDLET_ROOT = "$PROJECT_ROOT\Cmdlet"

[string[]]$Private:MANIFEST = (
  Import-PowerShellDataFile -Path $PROJECT_ROOT\Data\Cmdlet.psd1
).Cmdlet

[hashtable]$Private:Install = @{
  Destination = $CMDLET_ROOT
  Force       = $True
}
foreach ($Private:Project in $MANIFEST) {
  [hashtable]$Private:Built = @{
    Path = "$CMDLET_ROOT\$Project\bin\Release\netstandard2.0\$Project.dll"
  }
  if (Test-Path @Built) {
    [hashtable]$Private:Installation = @{
      Path = "$($Install.Destination)\$Project.dll"
    }
    if (
      -not (Test-Path @Installation) -or (
        Get-FileHash @Installation
      ).Hash -ne (
        Get-FileHash @Built
      ).Hash
    ) {
      Copy-Item @Built @Install
    }
  }
}

foreach ($Private:Type in $MANIFEST) {
  $Private:Assembly = @{
    Path = "$CMDLET_ROOT\$Type.dll"
  }
  if (Test-Path @Assembly -Leaf) {
    Add-Type @Assembly 
  }
}
