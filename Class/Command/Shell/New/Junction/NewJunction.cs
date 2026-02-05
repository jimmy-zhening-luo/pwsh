namespace Module.Command.Shell.New.Junction;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(IO.DirectoryInfo))]
public sealed class NewJunction : WrappedCommandShouldProcess
{
  public NewJunction() : base(
    "New-Item"
  )
  { }

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
    Position = 1,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Target")]
  [PathCompletions]
  public object? Value;

  private protected sealed override void TransformParameters()
  {
    BoundParameters["ItemType"] = "Junction";
    BoundParameters["Force"] = SwitchParameter.Present;
  }
}
