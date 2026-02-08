namespace Module.Command.Shell.New.Junction;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(IO.DirectoryInfo))]
public sealed class NewJunction() : WrappedCommandShouldProcess(
  "New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [PathCompletions]
  public string[] Path
  {
    get => paths;
    set => paths = value;
  }
  private string[] paths = [];

  [Parameter(
    Mandatory = true,
    Position = 1,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Target")]
  [PathCompletions]
  public object Value
  {
    get => initialValue;
    set => initialValue = value;
  }
  private object initialValue = "";

  private protected sealed override void TransformParameters()
  {
    BoundParameters["ItemType"] = "Junction";
    BoundParameters["Force"] = SwitchParameter.Present;
  }
}
