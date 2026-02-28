namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsDiagnostic.Measure,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-status"
)]
[Alias("gg")]
public sealed class GitMeasure() : GitCommand("status")
{
  private protected sealed override List<string> ParseArguments() => [];
}
