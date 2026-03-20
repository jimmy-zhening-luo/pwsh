namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-add"
)]
[Alias("ga")]
sealed public class GitAdd() : Git("add")
{
  const string FlagRenormalize = "--renormalize";

  const string DefaultPattern = Client.File.PathString.StringHere;

  [Parameter(
    Position = default,
    HelpMessage = $"File pattern of files to add, defaulting to '{DefaultPattern}' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Name
  {
    private get => field ?? DefaultPattern;
    init;
  }

  [Parameter(
    HelpMessage = $"Equivalent to {FlagRenormalize}"
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
