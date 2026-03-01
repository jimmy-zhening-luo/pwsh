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
  public string Script { get; set; } = string.Empty;

  private protected sealed override List<string> ParseArguments() => [Script];
}
