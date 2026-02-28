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

    if (d)
    {
      command.Add("-d");
    }
    if (e)
    {
      command.Add("-e");
    }
    if (i)
    {
      command.Add("-i");
    }
    if (o)
    {
      command.Add("-o");
    }
    if (p)
    {
      command.Add("-p");
    }
    if (v)
    {
      command.Add("-v");
    }


    return command;
  }
}
