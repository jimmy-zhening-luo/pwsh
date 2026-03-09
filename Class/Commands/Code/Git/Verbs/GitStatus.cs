namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsDiagnostic.Measure,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-status"
)]
[Alias("gg")]
sealed public class GitStatus() : GitCommand("status")
{ }
