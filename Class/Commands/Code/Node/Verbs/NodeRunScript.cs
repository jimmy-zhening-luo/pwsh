namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "NodePackageScript",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-run"
)]
[Alias("nr")]
sealed public class NodeRunScript() : NodeCommand("run")
{
  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Name of the npm script to run"
  )]
  [ValidateNotNullOrWhiteSpace]
  public required string Script
  {
    private get;
    set;
  }

  sealed private protected override List<string> ParseArguments() => [Script];
}
