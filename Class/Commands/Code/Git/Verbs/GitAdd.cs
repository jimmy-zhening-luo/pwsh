namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-add"
)]
[Alias("ga")]
sealed public class GitAdd() : GitCommand("add")
{
  const string FlagRenormalize = "--renormalize";

  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to add, defaulting to '.' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Name
  {
    private get => name ?? ".";
    set => name = value;
  }
  string? name;

  [Parameter(
    HelpMessage = "Equivalent to --renormalize"
  )]
  public SwitchParameter Renormalize
  { private get; init; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (Renormalize)
    {
      if (!NativeArguments.Contains(FlagRenormalize))
      {
        NativeArguments.AddLast(FlagRenormalize);
      }
    }
  }

  sealed override private protected IEnumerable<string> ParseArguments() => [Name];
}
