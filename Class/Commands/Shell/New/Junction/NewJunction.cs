namespace Module.Commands.Shell.New.Junction;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(System.IO.DirectoryInfo))]
public sealed class NewJunction() : WrappedCommandShouldProcess(
  "New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
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
    Mandatory = true,
    Position = 1
  )]
  [Alias("Target")]
  [PathCompletions]
  public object Value
  {
    get => initialValue;
    set => initialValue = value;
  }
  private object initialValue = string.Empty;

  private protected sealed override void TransformParameters()
  {
    MyInvocation.BoundParameters["ItemType"] = "Junction";
    MyInvocation.BoundParameters["Force"] = SwitchParameter.Present;
  }
}
