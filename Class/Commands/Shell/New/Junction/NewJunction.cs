namespace Module.Commands.Shell.New.Junction;

[Cmdlet(
  VerbsCommon.New,
  "Junction",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mj")]
[OutputType(typeof(System.IO.DirectoryInfo))]
public sealed class NewJunction() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [Tab.PathCompletions]
  public required string[] Path
  {
    private get;
    set;
  }

  [Parameter(
    Mandatory = true,
    Position = 1
  )]
  [Alias("Target")]
  [Tab.PathCompletions]
  public required object Value
  {
    private get;
    set;
  }

  private protected sealed override Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["ItemType"] = "Junction",
    ["Force"] = true,
  };
}
