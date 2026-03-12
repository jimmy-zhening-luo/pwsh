namespace PowerModule.Commands.Windows.App.Verbs;

[Cmdlet(
  VerbsCommon.Find,
  "WinGetApp",
  HelpUri = $"{WinGetHelpLink}search"
)]
[Alias("wgf")]
[OutputType(typeof(void))]
sealed public class WinGetFind() : WinGetCommand("search")
{
  sealed override private protected void PreprocessArguments()
  {
    if (Arguments.Count is 0)
    {
      IntrinsicVerb = "list";
    }
  }
}
