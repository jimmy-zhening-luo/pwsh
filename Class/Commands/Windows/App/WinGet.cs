namespace Module.Commands.Windows.App;

public abstract class WinGetCommand(string IntrinsicVerb) : NativeVerbCommand(IntrinsicVerb)
{
  sealed private protected override string CommandPath { get; } = Client.Environment.Known.Application.WinGet;

  sealed private protected override List<string> NativeCommandArguments() => [];

  private protected override List<string> NativeCommandVerbArguments() => [];
}
