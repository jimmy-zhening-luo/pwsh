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
  public string Name {
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

  private protected sealed override List<string> ParseArguments()
  {
    List<string> arguments = [Name];

    if (
      WorkingDirectory is not ""
      && GitWorkingDirectory.Resolve(
        Pwd(),
        Pwd()
      ) is not ""
      && GitWorkingDirectory.Resolve(
        Pwd(),
        WorkingDirectory
      ) is ""
    )
    {
      arguments.Add(WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    arguments.AddRange(ArgumentList);
    ArgumentList = [];

    if (
      renormalize
      && !arguments.Contains(FlagRenormalize)
    )
    {
      arguments.Add(FlagRenormalize);
    }

    return arguments;
  }
}
