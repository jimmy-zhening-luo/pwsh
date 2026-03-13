namespace PowerModule.Commands.Windows.App;

abstract public class WinGet(string IntrinsicVerb) : NativeCommand(
  Client.Environment.Application.WinGet,
  IntrinsicVerb
)
{
  private protected const string WinGetHelpLink = "https://learn.microsoft.com/en-us/windows/package-manager/winget/";

  sealed override private protected IList<string> CommandArguments
  { get; } = [];

  override private protected IList<string> VerbArguments
  { get; } = [];
}
