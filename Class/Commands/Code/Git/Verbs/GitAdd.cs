namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-add"
)]
[Alias("ga")]
public sealed class GitAdd() : GitCommand("add")
{
  private static string FlagRenormalize => "--renormalize";

  [Parameter(
    Position = default,
    HelpMessage = "File pattern of files to add, defaults to '.' (all)"
  )]
  [PathSpecCompletions]
  public string Name
  {
    get => name is "" ? "." : name;
    set => name = value.Trim();
  }
  private string name = string.Empty;

  [Parameter(
    HelpMessage = "Equivalent to git add --renormalize flag"
  )]
  public SwitchParameter Renormalize
  {
    get => renormalize;
    set => renormalize = value;
  }
  private bool renormalize;

  private protected sealed override void PreprocessArguments()
  {
    if (renormalize)
    {
      if (WorkingDirectory == FlagRenormalize)
      {
        WorkingDirectory = string.Empty;
      }
      else if (
        new List<string>(ArgumentList) is var arguments
        && arguments.Contains(FlagRenormalize)
      )
      {
        _ = arguments.Remove(FlagRenormalize);

        ArgumentList = [.. arguments];
      }
    }
  }

  private protected sealed override List<string> ParseArguments() => renormalize
    ? [Name, FlagRenormalize]
    : [Name];
}
