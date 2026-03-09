namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-add"
)]
[Alias("ga")]
sealed public class GitAdd() : GitCommand("add")
{
  private const string FlagRenormalize = "--renormalize";

  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to add, defaulting to '.' (all)"
  )]
  [ValidateNotNullOrWhiteSpace]
  [PathSpecCompletions]
  public string Name
  {
    private get => name is ""
      ? "."
      : name;
    set => name = value;
  }
  private string name = string.Empty;

  [Parameter(
    HelpMessage = "Equivalent to --renormalize"
  )]
  public SwitchParameter Renormalize { private get; set; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (Renormalize)
    {
      if (!NativeArguments.Contains(FlagRenormalize))
      {
        NativeArguments.Add(FlagRenormalize);
      }
    }
  }

  sealed override private protected List<string> ParseArguments() => [Name];
}
