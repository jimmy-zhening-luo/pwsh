namespace PowerModule.Commands.Windows.App;

abstract public class WinGetCommand(string IntrinsicVerb) : NativeVerbCommand(IntrinsicVerb)
{
  sealed override private protected string CommandPath
  { get; } = Client.Environment.Known.Application.WinGet;

  sealed override private protected string[] CommandArguments
  { get; } = [];

  override private protected string[] VerbArguments
  { get; } = [];
}
