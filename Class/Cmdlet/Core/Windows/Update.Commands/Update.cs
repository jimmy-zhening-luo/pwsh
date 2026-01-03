using System.Diagnostics;
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
      if (!Context.Ssh())
      {
        Process.Start(
          new ProcessStartInfo("ms-settings:windowsupdate")
          {
            UseShellExecute = true
          }
        );
      }
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
      if (!Context.Ssh())
      {
        Process.Start(
          new ProcessStartInfo("ms-windows-store://downloadsandupdates")
          {
            UseShellExecute = true
          }
        );
      }
    }
  }
}
