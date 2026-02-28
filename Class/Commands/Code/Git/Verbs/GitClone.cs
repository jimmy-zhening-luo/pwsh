namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Import,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-clone"
)]
[Alias("gitcl")]
public sealed class GitClone() : GitCommand("clone")
{
  private static string FlagRenormalize => "--renormalize";

  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Remote repository URL or 'org/repo'"
  )]
  [ValidateNotNullOrWhiteSpace]
  public string Repository
  {
    get => remote;
    set
    {
      var segments = value.Split(
        '/',
        System.StringSplitOptions.RemoveEmptyEntries
        | System.StringSplitOptions.TrimEntries
      );

      remote = segments switch
      {
        [string org, string repo] => $"{org}/{repo}",
        [string repo] => $"jimmy-zhening-luo/{repo}",
        _ => string.Empty,
      };
    }
  }
  private string remote = string.Empty;

  [Parameter(
    HelpMessage = "Use git@github.com remote protocol instead of the default HTTPS"
  )]
  [Alias("ssh")]
  public SwitchParameter ForceSsh
  {
    get => forceSsh;
    set => forceSsh = value;
  }
  private bool forceSsh;

  private protected sealed override List<string> ParseArguments()
  {
    if (Repository is "")
    {
      Throw("No repository name given.");
    }

    return [
      string.Concat(
        forceSsh
          ? "git@github.com:"
          : "https://github.com/",
        Repository
      ),
    ];
  }
}
