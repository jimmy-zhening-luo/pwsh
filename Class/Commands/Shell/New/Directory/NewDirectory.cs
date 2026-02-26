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
  @"Microsoft.PowerShell.Management\New-Item"
)
{
  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [Parameter(
    ParameterSetName = "nameSet",
    Position = default
  )]
  [PathCompletions]
  public required string[] Path { get; set; }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  public required string Name { get; set; }

  [Parameter]
  [Alias("Target")]
  [PathCompletions]
  public required object Value { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["ItemType"] = "Directory",
  };
}
