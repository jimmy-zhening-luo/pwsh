namespace Module.Commands.Shell.New.Directory;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
public sealed class NewDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item",
  AcceptsPipelineInput: true
)
{
  private protected sealed override object? PipelineInput => Value;

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
  public required string[] Path
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  public required string Name
  {
    private get;
    set;
  }

  [Parameter(ValueFromPipeline = true)]
  [Alias("Target")]
  [Tab.PathCompletions]
  public required object Value { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    private get;
    set;
  }

  private protected sealed override Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["ItemType"] = "Directory",
  };
}
