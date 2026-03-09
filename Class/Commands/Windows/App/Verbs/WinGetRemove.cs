namespace PowerModule.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Remove,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall"
)]
[Alias("wgr")]
[OutputType(typeof(void))]
sealed public class WinGetRemove() : WinGetCommand("uninstall")
{ }
