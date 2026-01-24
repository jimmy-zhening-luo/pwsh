namespace Module.Command.Shell.Remove.Directory.Commands;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
public class RemoveDirectory : WrappedRemoveDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions]
  public new string[]? Path;

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
  public new string? Filter;
}
