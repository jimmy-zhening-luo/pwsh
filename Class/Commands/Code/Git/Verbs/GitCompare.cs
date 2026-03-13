namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-diff"
)]
[Alias("gd")]
sealed public class GitCompare() : Git("diff")
{
  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to diff, defaulting to '.' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Name
  { private get; init; } = string.Empty;

  sealed override private protected IList<string> ParseArguments() => Name is ""
    ? []
    : [Name];
}
