namespace PowerModule.Commands.Shell.Create;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
sealed public class NewDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item",
  "Value"
)
{
  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
  {
    ["ItemType"] = "Directory",
  };

  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [Parameter(
    ParameterSetName = "nameSet",
    Position = default
  )]
  [Tab.PathCompletions]
  required public string[] Path
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  required public string Name
  {
    init => Discard();
  }

  [Parameter(ValueFromPipeline = true)]
  [Alias("Target")]
  [Tab.PathCompletions]
  required public object Value
  { get; init; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => Discard();
  }
}
