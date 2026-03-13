namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = NpmHelpLink
)]
[Alias("n")]
sealed public class NodeVerbCommand() : NodeCommand(default)
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

  sealed override private protected IEnumerable<string> ParseArguments()
  {
    if (V && IntrinsicVerb is null)
    {
      V = default;

      return ["-v"];
    }
    else
    {
      return [];
    }
  }
}
