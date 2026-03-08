namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Compare,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-diff"
)]
[Alias("gd")]
public sealed class GitCompare() : GitCommand("diff")
{
  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to diff, defaulting to '.' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [PathSpecCompletions]
  public string Name
  {
    private get;
    set;
  } = string.Empty;

  private protected sealed override List<string> ParseArguments() => Name is "" ? [] : [Name];
}
