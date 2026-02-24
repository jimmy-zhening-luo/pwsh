namespace Module.Commands.Shell.New.Directory;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
public sealed class NewDirectory() : WrappedCommandShouldProcess(
  "New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = 0
  )]
  [Parameter(
    ParameterSetName = "nameSet",
    Position = 0
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
    Mandatory = true
  )]
  public string Name
  {
    get => name;
    set => name = value;
  }
  private string name = string.Empty;

  [Parameter]
  [Alias("Target")]
  [PathCompletions]
  public object Value
  {
    get => initialValue;
    set => initialValue = value;
  }
  private object initialValue = new();

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  private protected sealed override void TransformParameters() => MyInvocation.BoundParameters["ItemType"] = "Directory";
}
