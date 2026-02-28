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
  private protected sealed override List<string> ParseWinGetCommand()
  {
    List<string> arguments = [];

    if (ArgumentList is [])
    {
      arguments.Add("upgrade");
    }
    else
    {
      arguments.Add("install");
      arguments.AddRange(ArgumentList);
    }

    return arguments;
  }
}
