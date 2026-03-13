namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Import,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-clone"
)]
[Alias("gitcl")]
sealed public class GitClone() : Git("clone")
{
  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Remote repository URL or 'org/repo'"
  )]
  [ValidateNotNullOrWhiteSpace]
  public string Repository
  {
    private get => remote;
    set => remote = value?.Split(
      Client.File.PathString.AltSeparator,
      System.StringSplitOptions.RemoveEmptyEntries
      | System.StringSplitOptions.TrimEntries
    ) switch
    {
      [string org, string repo] => $"{org}/{repo}",
      [string repo] => $"jimmy-zhening-luo/{repo}",
      _ => string.Empty
    };
  }
  string remote = string.Empty;

  [Parameter(
    HelpMessage = "Use git@github.com remote protocol instead of HTTPS"
  )]
  [Alias("ssh")]
  public SwitchParameter ForceSsh
  { private get; init; }

  sealed override private protected void PreprocessOtherArguments() => System.ArgumentException.ThrowIfNullOrEmpty(
    Repository,
    nameof(Repository)
  );

  sealed override private protected IEnumerable<string> ParseArguments() => [
    string.Concat(
      ForceSsh
        ? "git@github.com:"
        : "https://github.com/",
      Repository
    ),
  ];
}
