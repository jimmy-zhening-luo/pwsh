namespace Core.Windows.Update.Commands
{
  using System.Management.Automation;
  using Context;

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
      Context.ShellExecute(
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
      Context.ShellExecute(
        "ms-windows-store://downloadsandupdates"
      );
    }
  }
}
