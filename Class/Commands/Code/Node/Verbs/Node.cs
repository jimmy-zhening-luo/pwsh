namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
public sealed class Node : NodeCommand
{
  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [NodeVerbCompletions]
  public string Verb
  {
    get => IntrinsicVerb;
    set => IntrinsicVerb = value;
  }

  new public SwitchParameter V { get; set; }

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => base.V;
    set => base.V = value;
  }

  private protected sealed override List<string> ParseArguments()
  {
    if (Version && Verb is "")
    {
      Version = false;

      return ["-v"];
    }
    else
    {
      return [];
    }
  }
}
