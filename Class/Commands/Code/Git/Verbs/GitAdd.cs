namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-add"
)]
[Alias("ga")]
sealed public class GitAdd() : Git("add")
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
    private get => field ?? ".";
    init;
  }

  [Parameter(
    HelpMessage = "Equivalent to --renormalize"
  )]
  public SwitchParameter Renormalize
  { private get; init; }

  sealed override private protected void FinishSetup()
  {
    if (Renormalize)
    {
      if (!NativeArguments.Contains(FlagRenormalize))
      {
        _ = NativeArguments.AddLast(FlagRenormalize);
      }
    }
  }

  sealed override private protected string[] GetVerbBaseArguments() => [Name];
}
