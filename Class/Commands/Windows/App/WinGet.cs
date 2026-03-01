namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : NativeCommand
{
  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand() => [
    Client.Environment.Known.Application.WinGet,
    .. ParseArguments(),
  ];
}
