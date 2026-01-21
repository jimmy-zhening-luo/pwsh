namespace Module.Command.Shell.New.Junction;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(DirectoryInfo))]
public class NewJunction : WrappedCommand
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [PathCompletions]
  public string[]? Path;

  [Parameter(
    Mandatory = true,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Target")]
  [PathCompletions]
  public object? Value;

  protected override string WrappedCommandName => "New-Item";

  protected override bool BeforeBeginProcessing()
  {
    BoundParameters["ItemType"] = "Junction";
    BoundParameters["Force"] = SwitchParameter.Present;

    return true;
  }
}
