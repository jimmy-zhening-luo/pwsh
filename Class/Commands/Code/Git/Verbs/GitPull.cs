namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-pull"
)]
[Alias("gp")]
sealed public class GitPull() : Git("pull");
