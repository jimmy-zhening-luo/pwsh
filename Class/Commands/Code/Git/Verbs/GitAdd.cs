namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-add"
)]
[Alias("ga")]
public sealed class GitAdd() : GitCommand("add")
{
  private const string FlagRenormalize = "--renormalize";

  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to add, defaulting to '.' (all)"
  )]
  [PathSpecCompletions]
  public string Name
  {
    get => name is "" ? "." : name;
    set => name = value;
  }
  private string name = string.Empty;

  [Parameter(
    HelpMessage = "Equivalent to --renormalize"
  )]
  public SwitchParameter Renormalize { get; set; }

  private protected sealed override void PreprocessArguments()
  {
    if (Renormalize)
    {
      if (WorkingDirectory == FlagRenormalize)
      {
        WorkingDirectory = string.Empty;
      }
      else if (!NativeArguments.Contains(FlagRenormalize))
      {
        NativeArguments.Add(FlagRenormalize);
      }
    }
  }

  private protected sealed override List<string> ParseArguments() => [Name];
}
