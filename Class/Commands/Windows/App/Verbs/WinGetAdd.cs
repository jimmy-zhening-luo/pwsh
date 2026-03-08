namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Add,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade"
)]
[Alias("wga")]
[OutputType(typeof(void))]
sealed public class WinGetAdd() : WinGetCommand("install")
{
  sealed override private protected void PreprocessArguments()
  {
    if (Arguments is [])
    {
      IntrinsicVerb = "upgrade";
    }
  }
}
