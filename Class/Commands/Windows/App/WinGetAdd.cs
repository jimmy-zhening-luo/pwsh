namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Add,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade"
)]
[Alias("wga")]
[OutputType(typeof(void))]
public sealed class WinGetAdd : WinGetCommand
{
  private protected sealed override List<string> ParseArguments() => ArgumentList is []
    ? "upgrade"
    : "install";
}
