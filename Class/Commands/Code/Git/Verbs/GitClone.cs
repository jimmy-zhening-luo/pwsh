namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Import,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-clone"
)]
[Alias("gitcl")]
public sealed class GitClone() : GitCommand("clone")
{
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
    HelpMessage = "Use git@github.com remote protocol instead of HTTPS"
  )]
  [Alias("ssh")]
  public SwitchParameter ForceSsh { get; set; }

  private protected sealed override void PreprocessArguments()
  {
    if (Repository is "")
    {
      ThrowError("No repository name given.");
    }
  }

  private protected sealed override List<string> ParseArguments() => [
    string.Concat(
      ForceSsh
        ? "git@github.com:"
        : "https://github.com/",
      Repository
    ),
  ];
}
