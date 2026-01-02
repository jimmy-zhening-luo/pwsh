using System.Diagnostics;
using System.Management.Automation;

namespace Core
{
  namespace Windows
  {
    namespace Commands
    {
      [Cmdlet(
        VerbsData.Update,
        "Windows"
      )]
      [Alias("wu")]
      [OutputType(typeof(void))]
      public class UpdateWindows : PSCmdlet
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
    }
  } // namespace Windows
} // namespace Core
