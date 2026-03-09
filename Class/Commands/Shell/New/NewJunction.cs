namespace Module.Commands.Shell.New;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(System.IO.DirectoryInfo))]
sealed public class NewJunction() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item",
  AcceptsPipelineInput: true
)
{
  sealed override private protected object? PipelineInput => Value;

  sealed override private protected Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["ItemType"] = "Junction",
    ["Force"] = true,
  };

  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [Tab.PathCompletions]
  required public string[] Path { private get; set; }

  [Parameter(
    Mandatory = true,
    Position = 1,
    ValueFromPipeline = true
  )]
  [Alias("Target")]
  [Tab.PathCompletions]
  required public object Value { get; set; }
}
