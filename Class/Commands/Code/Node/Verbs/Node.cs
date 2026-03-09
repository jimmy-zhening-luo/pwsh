namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
sealed public class NodeVerbCommand() : NodeCommand(default)
{
  new private SwitchParameter V { get; set; }

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

  sealed override private protected string[] ParseArguments()
  {
    if (V && IntrinsicVerb is null)
    {
      V = false;

      return ["-v"];
    }
    else
    {
      return [];
    }
  }
}
