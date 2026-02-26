namespace Module.Commands.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "Windows"
)]
[Alias("wu")]
[OutputType(typeof(void))]
public sealed class UpdateWindows() : CoreCommand(
  true
)
{
  private protected sealed override void Postprocess()
  {
    Client.Start.ShellExecute(
      "ms-settings:windowsupdate"
    );
  }
}
