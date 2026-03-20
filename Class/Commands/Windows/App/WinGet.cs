namespace PowerModule.Commands.Windows.App;

abstract public class WinGet(string IntrinsicVerb) : NativeCommand(
  "winget",
  IntrinsicVerb
)
{
  private protected const string WinGetNoun = "WinGetApp";
  private protected const string WinGetHelpLink = "https://learn.microsoft.com/en-us/windows/package-manager/winget/";
}
