namespace PowerModule.Commands.Code.Node.Cli;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = NpmHelpLink
)]
[Alias("n")]
sealed public class NpmCli() : Npm(default)
{
  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [ValidateNotNullOrWhiteSpace]
  [NpmVerbCompletions]
  public string Verb
  {
    set => IntrinsicVerb = value;
  }

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    set => base.V = value;
  }
}
