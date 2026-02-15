namespace Module.Commands.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "StoreApp"
)]
[Alias("su")]
[OutputType(typeof(void))]
public sealed class UpdateStoreApp() : CoreCommand(
  true
)
{
  private protected sealed override void AfterEndProcessing()
  {
    PC.Invocation.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
