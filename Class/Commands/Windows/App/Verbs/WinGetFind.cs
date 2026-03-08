namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Find,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/search"
)]
[Alias("wgf")]
[OutputType(typeof(void))]
sealed public class WinGetFind() : WinGetCommand("search")
{
  sealed override private protected void PreprocessArguments()
  {
    if (Arguments is [])
    {
      IntrinsicVerb = "list";
    }
  }
}
