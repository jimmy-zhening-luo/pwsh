namespace Module.Commands.Shell.Remove.Directory;

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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath { get; set; } = [];
}
