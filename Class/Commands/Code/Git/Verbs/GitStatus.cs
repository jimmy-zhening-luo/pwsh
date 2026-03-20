namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsDiagnostic.Measure,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-status"
)]
[Alias("gg")]
sealed public class GitStatus() : Git("status");
