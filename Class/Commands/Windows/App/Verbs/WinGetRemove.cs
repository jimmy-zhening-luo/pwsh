namespace PowerModule.Commands.Windows.App.Verbs;

[Cmdlet(
  VerbsCommon.Remove,
  WinGetNoun,
  HelpUri = $"{WinGetHelpLink}uninstall"
)]
[Alias("wgr")]
[OutputType(typeof(void))]
sealed public class WinGetRemove() : WinGet("uninstall");
