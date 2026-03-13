namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsDiagnostic.Test,
  "NodePackage",
  HelpUri = $"{NpmHelpLink}/npm-test"
)]
[Alias("nt")]
sealed public class NpmTest() : Npm("test")
{
  const string FlagIgnoreScript = "--ignore-scripts";

  new public SwitchParameter I
  { get; set; }

  [Parameter(
    HelpMessage = "Do not run scripts (--ignore-scripts). Commands explicitly intended to run a particular script, such as npm start, npm stop, npm restart, npm test, and npm run-script will still run their intended script if ignore-scripts is set, but they will not run any pre- or post-scripts."
  )]
  [Alias("i")]
  public SwitchParameter IgnoreScript
  { private get; init; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (IgnoreScript && !NativeArguments.Contains(FlagIgnoreScript))
    {
      _ = NativeArguments.AddFirst(FlagIgnoreScript);
    }
  }
}
