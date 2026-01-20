namespace Module.Command.Windows.Update
{
  namespace Commands
  {

    [Cmdlet(
      VerbsData.Update,
      "Windows"
    )]
    [Alias("wu")]
    [OutputType(typeof(void))]
    public class UpdateWindows : Cmdlet
    {
      protected override void EndProcessing()
      {
        ShellExecute(
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
    public class UpdateStoreApp : Cmdlet
    {
      protected override void EndProcessing()
      {
        ShellExecute(
          "ms-windows-store://downloadsandupdates"
        );
      }
    }
  }
}
