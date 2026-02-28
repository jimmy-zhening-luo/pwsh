namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsDiagnostic.Test,
  "NodePackage",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-test"
)]
[Alias("nt")]
public sealed class NodeTest() : NpmCommand("test")
{
  private static string FlagIgnoreScript => "--ignore-scripts";

  new public SwitchParameter I { get; set; }

  [Parameter(
    HelpMessage = "Do not run scripts (--ignore-scripts). Commands explicitly intended to run a particular script, such as npm start, npm stop, npm restart, npm test, and npm run-script will still run their intended script if ignore-scripts is set, but they will not run any pre- or post-scripts."
  )]
  [Alias("i")]
  public SwitchParameter IgnoreScript
  {
    get => ignoreScript;
    set => ignoreScript = value;
  }
  private bool ignoreScript;

  private protected sealed override List<string> ParseArguments() => ignoreScript && !ArgumentList.Contains(FlagIgnoreScript)
    ? [FlagIgnoreScript]
    : [];
}
