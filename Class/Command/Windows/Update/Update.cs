namespace Module.Command.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "Windows"
)]
[Alias("wu")]
[OutputType(typeof(void))]
public sealed class UpdateWindows : Cmdlet
{
  protected sealed override void EndProcessing()
  {
    Invocation.ShellExecute(
      "ms-settings:windowsupdate"
    );
  }
}

[Cmdlet(
  VerbsData.Update,
  "StoreApp"
)]
[Alias("su")]
[OutputType(typeof(void))]
public sealed class UpdateStoreApp : Cmdlet
{
  protected sealed override void EndProcessing()
  {
    Invocation.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
