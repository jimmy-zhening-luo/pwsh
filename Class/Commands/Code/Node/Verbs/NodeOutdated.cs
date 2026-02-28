namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "NodeModule",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-outdated"
)]
[Alias("npo")]
public sealed class NodeOutdated() : NpmCommand("outdated")
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

  private protected sealed override List<string> ParseArguments()
  {
    noThrow = true;

    return all && !(new List<string>(ArgumentList)).Contains(FlagAll)
      ? [FlagAll]
      : [];
  }
}
