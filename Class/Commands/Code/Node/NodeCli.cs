namespace PowerModule.Commands.Code.Node;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = NpmHelpLink
)]
[Alias("n")]
sealed public class NodeCli() : Node(default)
{
  new public SwitchParameter V
  { get; set; }

  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [ValidateNotNullOrWhiteSpace]
  [NodeVerbCompletions]
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
