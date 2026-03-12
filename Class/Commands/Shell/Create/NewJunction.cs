namespace PowerModule.Commands.Shell.Create;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = $"{HelpLink}2096592"
)]
[Alias("mj")]
[OutputType(typeof(System.IO.DirectoryInfo))]
sealed public class NewJunction() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => Value;

  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
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
  required public string[] Path
  {
    init => Discard();
  }

  [Parameter(
    Mandatory = true,
    Position = 1,
    ValueFromPipeline = true
  )]
  [Alias("Target")]
  [Tab.PathCompletions]
  required public object Value
  { get; init; }
}
