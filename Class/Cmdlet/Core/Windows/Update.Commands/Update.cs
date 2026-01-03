using System.Management.Automation;

namespace Core.Windows.Update.Commands
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
      Context.Start(
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
      Context.Start(
        "ms-windows-store://downloadsandupdates"
      );
    }
  }
}
