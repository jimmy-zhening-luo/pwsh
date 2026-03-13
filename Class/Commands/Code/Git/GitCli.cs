namespace PowerModule.Commands.Code.Git;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = GitHelpLink
)]
[Alias("g")]
sealed public class GitCli() : Git(default)
{
  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = "Git command"
  )]
  [ValidateNotNullOrWhiteSpace]
  [GitVerbCompletions]
  public string Verb
  {
    set => IntrinsicVerb = value;
  }

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    set => base.V = value;
  }
}
