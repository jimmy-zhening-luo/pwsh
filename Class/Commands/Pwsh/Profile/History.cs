namespace Module.Commands.Pwsh.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "History"
)]
[Alias("oc")]
[OutputType(typeof(void))]
public sealed class StartHistory() : CoreCommand(
  true
)
{
  private protected sealed override void Postprocess() => Client.Invocation.CreateProcess(
    Client.Environment.Known.Application.VSCode,
    [
      Client.Environment.Known.Folder.AppData(
        @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt"
      ),
      "--profile=Setting",
      "--new-window",
    ],
    true
  );
}
