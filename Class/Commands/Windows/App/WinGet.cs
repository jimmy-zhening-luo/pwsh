namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : NativeCommand
{
  private protected sealed override string CommandPath => Client.Environment.Known.Application.WinGet;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand() => ParseArguments();
}
