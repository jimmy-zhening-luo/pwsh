namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = GitHelpLink
)]
[Alias("g")]
sealed public class GitVerbCommand() : GitCommand(default)
{
  new public SwitchParameter V
  { get; set; }

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
