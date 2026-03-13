namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "NodePackageScript",
  HelpUri = $"{NpmHelpLink}/npm-run"
)]
[Alias("nr")]
sealed public class NpmRunScript() : Npm("run")
{
  [Parameter(
    Mandatory = true,
    Position = default,
    HelpMessage = "Name of the npm script to run"
  )]
  [ValidateNotNullOrWhiteSpace]
  required public string Script
  { private get; init; }

  sealed override private protected IList<string> ParseArguments() => [Script];
}
