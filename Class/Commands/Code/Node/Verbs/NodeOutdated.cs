namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "NodeModule",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-outdated"
)]
[Alias("npo")]
public sealed class NodeOutdated() : NodeCommand("outdated")
{
  private const string FlagAll = "--all";

  [Parameter(
    HelpMessage = "In addition to direct dependencies, check for outdated meta-dependencies (--all)"
  )]
  [Alias("a")]
  public SwitchParameter All { get; set; }

  private protected sealed override void PreprocessArguments()
  {
    NoThrow = true;

    if (All && !NativeArguments.Contains(FlagAll))
    {
      NativeArguments.Insert(default, FlagAll);
    }
  }
}
