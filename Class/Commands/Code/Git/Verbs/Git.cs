namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = "https://git-scm.com/docs"
)]
[Alias("g")]
sealed public class Git() : GitCommand(default)
{
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

  new private SwitchParameter V { get; set; }

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    set => base.V = value;
  }
}
