namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Compare,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-diff"
)]
[Alias("gd")]
sealed public class GitCompare() : Git("diff")
{
  [Parameter(Position = default)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Path
  { private get; init; } = string.Empty;

  sealed override private protected string[] GetVerbBaseArguments() => Path is ""
    ? []
    : [Path];
}
