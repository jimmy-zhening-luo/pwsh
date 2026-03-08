namespace Module.Commands.Windows.App;

abstract public class WinGetCommand(string IntrinsicVerb) : NativeVerbCommand(IntrinsicVerb)
{
  sealed override private protected string CommandPath { get; } = Client.Environment.Known.Application.WinGet;

  sealed override private protected List<string> NativeCommandArguments { get; } = [];

  override private protected List<string> NativeCommandVerbArguments { get; } = [];
}
