namespace Module.Command.Windows.Update;

[Cmdlet(
  VerbsData.Update,
  "Windows"
)]
[Alias("wu")]
[OutputType(typeof(void))]
public sealed class UpdateWindows : CoreCommand
{
  private protected sealed override bool SkipSsh => true;

  private protected sealed override void AfterEndProcessing()
  {
    Invocation.ShellExecute(
      "ms-settings:windowsupdate"
    );
  }
}
