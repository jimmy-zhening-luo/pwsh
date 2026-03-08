namespace Module.Commands.Pwsh.History;

[Cmdlet(
  VerbsLifecycle.Start,
  "History"
)]
[Alias("oc")]
[OutputType(typeof(void))]
sealed public class StartHistory() : CoreCommand(true)
{
  sealed override private protected void Postprocess() => Client.File.Handler.Edit(
    Client.Environment.Known.Folder.AppData(
      @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt"
    ),
    "Setting",
    Client.File.Handler.EditorWindow.New
  );
}
