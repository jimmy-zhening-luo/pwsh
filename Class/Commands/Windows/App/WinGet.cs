namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : NativeCommand
{
  private protected abstract List<string> ParseArguments();

  private protected sealed override CommandArguments BuildNativeCommand() => new(
    [
      "&",
      Client.Environment.Known.Application.WinGet,
      .. ParseWinGetCommand(),
    ],
    []
  );
}
