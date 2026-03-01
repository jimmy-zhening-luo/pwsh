namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Remove,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall"
)]
[Alias("wgr")]
[OutputType(typeof(void))]
public sealed class WinGetRemove() : WinGetCommand("uninstall")
{
  private protected sealed override List<string> NativeCommandVerbArguments() => [];
}
