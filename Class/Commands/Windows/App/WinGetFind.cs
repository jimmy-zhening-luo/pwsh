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
  private protected sealed override List<string> ParseWinGetCommand()
  {
    List<string> arguments = [];

    if (ArgumentList is [])
    {
      arguments.Add("list");
    }
    else
    {
      arguments.Add("search");
      arguments.AddRange(ArgumentList);
    }

    return arguments;
  }
}
