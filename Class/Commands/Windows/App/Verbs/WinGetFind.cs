namespace PowerModule.Commands.Windows.App.Verbs;

[Cmdlet(
  VerbsCommon.Find,
  "WinGetApp",
  HelpUri = $"{WinGetHelpLink}search"
)]
[Alias("wgf")]
[OutputType(typeof(void))]
sealed public class WinGetFind() : WinGet("search")
{
  sealed override private protected void Setup()
  {
    if (Arguments.Count is 0)
    {
      IntrinsicVerb = "list";
    }
  }
}
