namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-pull"
)]
[Alias("gp")]
sealed public class GitPull() : Git("pull")
{ }
