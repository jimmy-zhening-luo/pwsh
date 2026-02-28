namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Find,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/search"
)]
[Alias("wgf")]
[OutputType(typeof(void))]
public sealed class WinGetFind : WinGetCommand
{
  private protected sealed override List<string> ParseArguments() => ArgumentList is []
    ? "list"
    : "search";
}
