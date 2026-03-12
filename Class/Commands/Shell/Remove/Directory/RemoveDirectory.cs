namespace PowerModule.Commands.Shell.Remove.Directory;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
sealed public class RemoveDirectory : WrappedRemoveDirectory
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  required sealed override public string[] Path
  { get; init; }

  [Parameter(
    ParameterSetName = StandardParameter.LiteralPath,
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => Discard();
  }
}
