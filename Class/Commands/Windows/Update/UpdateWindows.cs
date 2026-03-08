namespace Module.Commands.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "Windows"
)]
[Alias("wu")]
[OutputType(typeof(void))]
sealed public class UpdateWindows() : CoreCommand(true)
{
  sealed override private protected void Postprocess()
  {
    Client.Start.ShellExecute(
      "ms-settings:windowsupdate"
    );
  }
}
