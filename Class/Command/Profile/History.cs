namespace Module.Command.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "History"
)]
[Alias("oc")]
[OutputType(typeof(void))]
public sealed class StartHistory : Cmdlet
{
  protected sealed override void EndProcessing() => CreateProcess(
    Application.VSCode,
    [
      AppData(
        @"Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt"
      ),
      "--profile=Setting",
      "--new-window"
    ],
    true
  );
}
