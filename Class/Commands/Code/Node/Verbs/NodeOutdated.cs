namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "NodeModule",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-outdated"
)]
[Alias("npo")]
sealed public class NodeOutdated() : NodeCommand("outdated")
{
  private const string FlagAll = "--all";

  [Parameter(
    HelpMessage = "In addition to direct dependencies, check for outdated meta-dependencies (--all)"
  )]
  [Alias("a")]
  public SwitchParameter All
  {
    private get;
    set;
  }

  sealed override private protected void PreprocessOtherArguments()
  {
    NoThrow = true;

    if (All && !NativeArguments.Contains(FlagAll))
    {
      NativeArguments.Insert(default, FlagAll);
    }
  }
}
