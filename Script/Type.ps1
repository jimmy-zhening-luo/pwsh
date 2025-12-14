$Private:CMDLET_ROOT = "$PSScriptRoot\..\Cmdlet"

$Private:Types = @(
  'CompleterBase.dll'
)

foreach ($Private:Type in $Types) {
  Add-Type -Path "$CMDLET_ROOT\$Type"
}
