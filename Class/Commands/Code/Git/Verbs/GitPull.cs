namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-pull"
)]
[Alias("gp")]
sealed public class GitPull() : GitCommand("pull")
{ }
