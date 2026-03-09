namespace PowerModule.Commands.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "StoreApp"
)]
[Alias("su")]
[OutputType(typeof(void))]
sealed public class UpdateStoreApp() : CoreCommand(true)
{
  sealed override private protected void Postprocess()
  {
    Client.Start.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
