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
  @"Microsoft.PowerShell.Management\New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [PathCompletions]
  public string[] Path { get; set; } = [];

  [Parameter(
    Mandatory = true,
    Position = 1
  )]
  [Alias("Target")]
  [PathCompletions]
  public object Value { get; set; } = string.Empty;

  private protected sealed override void TransformArguments()
  {
    BoundParameters["ItemType"] = "Junction";
    BoundParameters["Force"] = SwitchParameter.Present;
  }
}
