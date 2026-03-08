namespace Module.Commands.Pwsh.History;

[Cmdlet(
  VerbsLifecycle.Start,
  "History"
)]
[Alias("oc")]
[OutputType(typeof(void))]
sealed public class StartHistory() : CoreCommand(true)
{
  sealed private protected override void Postprocess() => Client.Start.CreateProcess(
    Client.Environment.Known.Application.VSCode,
    [
      Client.Environment.Known.Folder.AppData(
        @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt"
      ),
      "--profile=Setting",
      "--new-window",
    ]
  );
}
