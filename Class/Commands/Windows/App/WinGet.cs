namespace Module.Commands.Windows.App;

public abstract class WinGetCommand(
  string IntrinsicVerb = ""
) : NativeVerbCommand(IntrinsicVerb)
{
  private protected sealed override string CommandPath => Client.Environment.Known.Application.WinGet;

  private protected sealed override List<string> NativeCommandArguments() => [];
}
