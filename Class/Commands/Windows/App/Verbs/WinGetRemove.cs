namespace PowerModule.Commands.Windows.App.Verbs;

[Cmdlet(
  VerbsCommon.Remove,
  "WinGetApp",
  HelpUri = $"{WinGetHelpLink}uninstall"
)]
[Alias("wgr")]
[OutputType(typeof(void))]
sealed public class WinGetRemove() : WinGet("uninstall")
{ }
