namespace Module.Commands.Windows.App;

public abstract class WinGetCommand : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  public string[] ArgumentList { get; set; } = [];

  private protected string WinGet => Client.Environment.Known.Application.WinGet;

  private protected abstract List<string> ParseWinGetCommand();

  private protected sealed override void Postprocess()
  {
    List<string> command = ["&", WinGet];

    command.AddRange(ParseWinGetCommand());

    AddScript(string.Join(' ', command));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      Throw("winget error");
    }
  }
}
