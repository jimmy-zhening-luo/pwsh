namespace PowerModule.Commands.Shell.Remove.Directory;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
sealed public class RemoveDirectory : WrappedRemoveDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  required sealed override public string[] Path
  { get; set; }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string[] LiteralPath
  { private get; set; }
}
