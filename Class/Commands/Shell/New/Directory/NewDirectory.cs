namespace Module.Commands.Shell.New.Directory;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
public sealed class NewDirectory() : WrappedCommand(
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
  [Tab.PathCompletions]
  public required string[] Path
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  public required string Name
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("Target")]
  [Tab.PathCompletions]
  public required object Value
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    private get;
    set;
  }

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["ItemType"] = "Directory",
  };
}
