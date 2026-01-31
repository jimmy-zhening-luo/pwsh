namespace Module.Command.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "Windows"
)]
[Alias("wu")]
[OutputType(typeof(void))]
public sealed class UpdateWindows : CoreCommand
{
  private protected sealed override bool NoSsh => true;

  private protected sealed override void AfterEndProcessing()
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
public sealed class UpdateStoreApp : CoreCommand
{
  private protected sealed override bool NoSsh => true;

  private protected sealed override void AfterEndProcessing()
  {
    Invocation.ShellExecute(
      "ms-windows-store://downloadsandupdates"
    );
  }
}
