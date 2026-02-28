namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
public sealed class Node : NodeCommand
{
  internal sealed class NodeVerbCompletionsAttribute() : CompletionsAttribute([.. Verbs]);

  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [NodeVerbCompletions]
  public string Verb { get; set; } = string.Empty;

  new public SwitchParameter V { get; set; }

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => version;
    set => version = value;
  }
  private bool version;

  private protected sealed override List<string> ParseArguments()
  {
    List<string> command = [];

    List<string> arguments = [.. ArgumentList];
    ArgumentList = [];

    if (WorkingDirectory is not "")
    {
      if (IsNodePackage(WorkingDirectory))
      {
        var packagePrefix = WorkingDirectory is ""
          ? ""
          : "--prefix="
            + Client.File.PathString.FullPathLocationRelative(
                Pwd(),
                WorkingDirectory
              );

        if (packagePrefix is not "")
        {
          command.Add(packagePrefix);
        }
      }
      else
      {
        arguments.Insert(default, WorkingDirectory);
      }
    }

    WorkingDirectory = string.Empty;

    if (
      Verb is not ""
      && Verb.StartsWith('-')
      || !Verbs.Contains(Verb.ToLower())
      && !Aliases.ContainsKey(Verb.ToLower())
    )
    {
      var deferredVerb = arguments is []
        ? ""
        : arguments.Find(
            argument => Verbs.Contains(
              argument.ToLower()
            )
          );

      if (deferredVerb is not (null or ""))
      {
        _ = arguments.Remove(deferredVerb);
      }

      arguments.Insert(default, Verb);

      Verb = deferredVerb ?? string.Empty;
    }

    if (Verb is not "")
    {
      IntrinsicVerb = Verb;

      if (version)
      {
        arguments.Add("-v");
      }
    }
    else
    {
      if (version)
      {
        command.Add("-v");
      }
    }

    command.AddRange(arguments);

    return command;
  }
}
