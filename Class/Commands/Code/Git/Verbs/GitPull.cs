namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-pull"
)]
[Alias("gp")]
public sealed class GitPull() : GitCommand("pull")
{
  private protected sealed override List<string> ParseArguments() => [];
}
