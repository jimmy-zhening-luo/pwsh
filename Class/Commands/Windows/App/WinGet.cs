namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : NativeCommand
{
  private protected abstract List<string> ParseWinGetCommand();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> command = [
      "&",
      Client.Environment.Known.Application.WinGet,
    ];

    command.AddRange(ParseWinGetCommand());

    return command;
  }
}
