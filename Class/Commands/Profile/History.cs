namespace Module.Commands.Profile;

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
  private protected sealed override void AfterEndProcessing() => Invocation.CreateProcess(
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
