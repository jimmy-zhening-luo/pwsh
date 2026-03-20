namespace PowerModule.Commands.Shell.Remove.Directory;

[Cmdlet(
  VerbsCommon.Remove,
  "Directory",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2097103"
)]
[Alias("rd")]
[OutputType(typeof(void))]
sealed public class RemoveDirectory : WrappedRemoveDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  required sealed override public string[] Path
  { get; init; }

  [Parameter(
    ParameterSetName = nameof(LiteralPath),
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => _ = value;
  }
}
