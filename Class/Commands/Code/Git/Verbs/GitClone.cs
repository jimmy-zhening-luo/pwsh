namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Import,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-clone"
)]
[Alias("gitcl")]
sealed public class GitClone() : Git("clone")
{
  const string GitBaseUrl = "github.com";
  const string GitProtocolHttp = $"https://{GitBaseUrl}/";
  const string GitProtocolSsh = $"git@{GitBaseUrl}:";

  const string DefaultOrganization = "jimmy-zhening-luo";

  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Remote repository URL or 'org/repo' shorthand"
  )]
  [ValidateNotNullOrWhiteSpace]
  required public string Repository
  {
    private get;
    init => field = value?.Split(
      Client.File.PathString.AltSeparator,
      System.StringSplitOptions.RemoveEmptyEntries
      | System.StringSplitOptions.TrimEntries
    )
    switch
    {
      [
        string org,
        string repo,
      ] => $"{org}/{repo}",
      [
        string repo,
      ] => $"{DefaultOrganization}/{repo}",
      _ => string.Empty
    };
  }

  [Parameter(
    HelpMessage = "Use SSH protocol instead of HTTP"
  )]
  [Alias("ssh")]
  public SwitchParameter ForceSsh
  { private get; init; }

  sealed override private protected void FinishSetup() => System.ArgumentException.ThrowIfNullOrEmpty(
    Repository,
    nameof(Repository)
  );

  sealed override private protected string[] GetVerbBaseArguments() => [
    string.Concat(
      ForceSsh
        ? GitProtocolSsh
        : GitProtocolHttp,
      Repository
    ),
  ];
}
