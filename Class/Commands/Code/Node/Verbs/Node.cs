namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
public sealed class Node() : NodeCommand(default)
{
  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [NodeVerbCompletions]
  public string Verb
  {
    set => IntrinsicVerb = value;
  }

  new private SwitchParameter V { get; set; }

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    set => base.V = value;
  }

  private protected sealed override List<string> ParseArguments()
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
