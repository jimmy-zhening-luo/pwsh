namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsDiagnostic.Test,
  "NodePackage",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-test"
)]
[Alias("nt")]
public sealed class NodeTest() : NodeCommand("test")
{
  private const string FlagIgnoreScript = "--ignore-scripts";

  new private SwitchParameter I { get; set; }

  [Parameter(
    HelpMessage = "Do not run scripts (--ignore-scripts). Commands explicitly intended to run a particular script, such as npm start, npm stop, npm restart, npm test, and npm run-script will still run their intended script if ignore-scripts is set, but they will not run any pre- or post-scripts."
  )]
  [Alias("i")]
  public SwitchParameter IgnoreScript
  {
    private get;
    set;
  }

  private protected sealed override void PreprocessArguments()
  {
    if (IgnoreScript && !NativeArguments.Contains(FlagIgnoreScript))
    {
      NativeArguments.Insert(default, FlagIgnoreScript);
    }
  }
}
