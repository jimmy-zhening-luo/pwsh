namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : NativeCommand
{
  private protected static string WinGet => Client.Environment.Known.Application.WinGet;

  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  public string[] ArgumentList { get; set; } = [];

  private protected abstract List<string> ParseWinGetCommand();

  private protected sealed override void BuildNativeCommand()
  {
    List<string> command = ["&", WinGet];

    command.AddRange(ParseWinGetCommand());

    AddScript(string.Join(' ', command));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      if (noThrow)
      {
        WriteWarning("winget error");
      }
      else
      {
        Throw("winget error");
      }
    }
  }
}
