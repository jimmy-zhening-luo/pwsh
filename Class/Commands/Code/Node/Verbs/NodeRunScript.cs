namespace PowerModule.Commands.Code.Node.Verbs;

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
  required public string Script
  { private get; set; }

  sealed override private protected string[] ParseArguments() => [Script];
}
