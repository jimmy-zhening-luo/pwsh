namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Find,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/search"
)]
[Alias("wgf")]
[OutputType(typeof(void))]
public sealed class WinGetFind() : WinGetCommand("search")
{
  private protected sealed override void PreprocessArguments()
  {
    if (ArgumentList is [])
    {
      IntrinsicVerb = "list";
    }
  }

  private protected sealed override List<string> NativeCommandVerbArguments() => [];
}
