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
  public required string[] Path { get; set; }

  [Parameter(
    Mandatory = true,
    Position = 1
  )]
  [Alias("Target")]
  [PathCompletions]
  public required object Value { get; set; }

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["ItemType"] = "Junction",
    ["Force"] = SwitchParameter.Present,
  };
}
