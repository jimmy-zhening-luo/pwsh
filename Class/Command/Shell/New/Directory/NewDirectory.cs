namespace Module.Command.Shell.New.Directory;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096592"
)]
[Alias("mk")]
[OutputType(typeof(DirectoryInfo))]
public class NewDirectory : WrappedCommand
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
  public string[]? Path;

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  public string? Name;

  [Parameter(
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Target")]
  [PathCompletions]
  public object? Value;

  [Parameter]
  [Alias("f")]
  public SwitchParameter? Force;

  private protected override bool BeforeBeginProcessing()
  {
    BoundParameters["ItemType"] = "Directory";

    return true;
  }
}
