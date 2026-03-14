namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = $"{NpmHelpLink}/npm-cache"
)]
[Alias("ncc")]
sealed public class NpmClearCache() : Npm("cache")
{
  sealed override private protected void FinishSetup()
  {
    WorkingDirectory = string.Empty;

    ClearArguments();

    AddLast("--force");
  }

  sealed override private protected string[] GetVerbBaseArguments() => ["clean"];
}
