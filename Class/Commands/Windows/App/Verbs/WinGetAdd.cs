namespace PowerModule.Commands.Windows.App.Verbs;

[Cmdlet(
  VerbsCommon.Add,
  "WinGetApp",
  HelpUri = $"{WinGetHelpLink}upgrade"
)]
[Alias("wga")]
[OutputType(typeof(void))]
sealed public class WinGetAdd() : WinGet("install")
{
  sealed override private protected void Setup()
  {
    if (Arguments.Count is 0)
    {
      IntrinsicVerb = "upgrade";
    }
  }
}
