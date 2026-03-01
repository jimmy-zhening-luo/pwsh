namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "NodeModule",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-outdated"
)]
[Alias("npo")]
public sealed class NodeOutdated() : NodeCommand("outdated")
{
  private static string FlagAll => "--all";

  [Parameter(
    HelpMessage = "In addition to direct dependencies, check for outdated meta-dependencies (--all)"
  )]
  [Alias("a")]
  public SwitchParameter All
  {
    get => all;
    set => all = value;
  }
  private bool all;

  private protected sealed override void PreprocessArguments()
  {
    noThrow = true;
  }

  private protected sealed override List<string> ParseArguments() => all && !new List<string>(ArgumentList).Contains(FlagAll)
    ? [FlagAll]
    : [];
}
