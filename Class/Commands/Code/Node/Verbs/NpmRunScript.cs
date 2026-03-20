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
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  required public string Name
  { private get; init; }

  sealed override private protected string[] GetVerbBaseArguments() => [Name];
}
