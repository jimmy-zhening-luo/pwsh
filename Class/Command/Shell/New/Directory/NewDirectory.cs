namespace Module.Command.Shell.New.Directory;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(IO.DirectoryInfo))]
public sealed class NewDirectory : WrappedCommandShouldProcess
{
  public NewDirectory() : base(
    "New-Item"
  )
  { }

  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [Parameter(
    ParameterSetName = "nameSet",
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
    ParameterSetName = "nameSet",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  public string Name
  {
    get => name;
    set => name = value;
  }
  private string name = "";

  [Parameter(
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Target")]
  [PathCompletions]
  public object Value
  {
    get => value;
    set => value = value;
  }
  private object value = null;

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force;

  private protected sealed override void TransformParameters() => BoundParameters["ItemType"] = "Directory";
}
