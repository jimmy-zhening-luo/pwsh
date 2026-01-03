using System.Diagnostics;
using System.Management.Automation;

namespace Core.Windows
{
  [Cmdlet(
    VerbsData.Edit,
    "SystemPath"
  )]
  [Alias("path")]
  [OutputType(typeof(void))]
  public class EditSystemPath : Cmdlet
  {
    [Parameter(
      HelpMessage = "Launch Environment Variables control panel as administrator to edit system variables, triggering a UAC prompt if needed."
    )]
    public SwitchParameter Administrator
    {
      get => administrator;
      set => administrator = value;
    }
    private bool administrator;

    protected override void EndProcessing()
    {
      if (!Context.Ssh())
      {
        var startInfo = new ProcessStartInfo(
          "rundll32.exe",
          [
            "sysdm.cpl",
            "EditEnvironmentVariables"
          ]
        );
        if (administrator)
        {
          startInfo.UseShellExecute = true;
          startInfo.Verb = "runas";
        }
        Process.Start(startInfo);
      }
    }
  }
}
