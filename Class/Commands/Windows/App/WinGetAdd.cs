namespace Module.Commands.Windows.App;

[Cmdlet(
  VerbsCommon.Add,
  "WinGetApp",
  HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade/"
)]
[Alias("wga")]
[OutputType(typeof(void))]
public sealed class WinGetAdd() : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  public string[] ArgumentList { get; set; } = [];

  private protected sealed override void Postprocess()
  {
    List<string> args = [
      "&",
       Client.Environment.Known.Application.WinGet
    ];

    if (ArgumentList is [])
    {
      args.Add("upgrade");
    }
    else
    {
      args.Add("install");
      args.AddRange(ArgumentList);
    }

    AddScript(string.Join(' ', ArgumentList));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      Throw("winget error");
    }
  }
}
