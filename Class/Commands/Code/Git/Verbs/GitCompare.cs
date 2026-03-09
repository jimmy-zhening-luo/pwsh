namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-diff"
)]
[Alias("gd")]
sealed public class GitCompare() : GitCommand("diff")
{
  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to diff, defaulting to '.' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Name
  { private get; set; } = string.Empty;

  sealed override private protected IEnumerable<string> ParseArguments() => Name is ""
    ? []
    : [Name];
}
