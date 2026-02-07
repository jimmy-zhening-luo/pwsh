namespace Module.Command.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "StoreApp"
)]
[Alias("su")]
[OutputType(typeof(void))]
public sealed class UpdateStoreApp : CoreCommand
{
  private protected sealed override bool SkipSsh => true;

  private protected sealed override void AfterEndProcessing()
  {
    Invocation.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
