namespace PowerModule.Commands.Pwsh.History;

[Cmdlet(
  VerbsLifecycle.Start,
  "History"
)]
[Alias("oc")]
[OutputType(typeof(void))]
sealed public class StartHistory() : CoreCommand(true)
{
  sealed override private protected void Postprocess() => Client.File.Editor.Edit(
    Client.Environment.Folder.AppData(
      @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt"
    ),
    Client.File.Editor.ProfileSetting,
    Client.File.Editor.Window.New
  );
}
