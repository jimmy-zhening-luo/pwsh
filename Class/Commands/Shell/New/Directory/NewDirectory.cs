namespace Module.Commands.Shell.New.Directory;

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
  sealed override private protected object? PipelineInput => Value;

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
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  required public string Name
  {
    private get;
    set;
  }

  [Parameter(ValueFromPipeline = true)]
  [Alias("Target")]
  [Tab.PathCompletions]
  required public object Value { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    private get;
    set;
  }

  sealed override private protected Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["ItemType"] = "Directory",
  };
}
