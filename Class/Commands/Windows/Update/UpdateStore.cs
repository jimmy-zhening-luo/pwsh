namespace Module.Commands.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "StoreApp"
)]
[Alias("su")]
[OutputType(typeof(void))]
public sealed class UpdateStoreApp() : CoreCommand(true)
{
  private protected sealed override void Postprocess()
  {
    Client.Start.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
