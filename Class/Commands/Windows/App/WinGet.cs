namespace PowerModule.Commands.Windows.App;

abstract public class WinGetCommand(string IntrinsicVerb) : NativeCommand(
  Client.Environment.Application.WinGet,
  IntrinsicVerb
)
{
  private protected const string WinGetHelpLink = "https://learn.microsoft.com/en-us/windows/package-manager/winget/";

  sealed override private protected IEnumerable<string> CommandArguments
  { get; } = [];

  override private protected IEnumerable<string> VerbArguments
  { get; } = [];
}
