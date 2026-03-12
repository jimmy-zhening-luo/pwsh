namespace PowerModule.Commands.Shell.Create;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
sealed public class NewDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item",
  AcceptsPipelineInput: true
)
{
  sealed override private protected object PipelineInput => Value;

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
