namespace Module.Command.Shell.Remove.Directory.Commands;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
public sealed class RemoveDirectory : WrappedRemoveDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Mandatory = true,
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath
  {
    get => literalPaths;
    set => literalPaths = value;
  }
  private string[] literalPaths = [];

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralPath",
    Position = 1
  )]
  [SupportsWildcards]
  public override sealed string Filter
  {
    get => filter;
    set => filter = value;
  }
}
