namespace Module.Command.Shell.Remove.Directory;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
public class RemoveDirectory : WrappedCommand
{
  [Parameter(
    ParameterSetName = "Path",
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [RelativePathCompletions]
  public string[]? Path;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralPath",
    Position = 1
  )]
  [SupportsWildcards]
  public string? Filter;

  [Parameter]
  [SupportsWildcards]
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  protected override string WrappedCommandName => "Remove-Item";

  protected override bool BeforeBeginProcessing()
  {
    BoundParameters["Recurse"] = SwitchParameter.Present;
    BoundParameters["Force"] = SwitchParameter.Present;

    return true;
  }
}
