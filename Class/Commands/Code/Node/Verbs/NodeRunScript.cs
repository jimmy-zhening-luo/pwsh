namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "NodePackageScript",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-run"
)]
[Alias("nr")]
public sealed class NodeRunScript() : NodeCommand("run")
{
  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Name of the npm script to run"
  )]
  [ValidateNotNullOrWhiteSpace]
  public string Script { get; set; } = "";

  private protected sealed override List<string> ParseArguments()
  {
    List<string> arguments = [Script];

    if (
      WorkingDirectory is not ""
      && !IsNodePackage(WorkingDirectory)
    )
    {
      arguments.Add(WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    return arguments;
  }
}
