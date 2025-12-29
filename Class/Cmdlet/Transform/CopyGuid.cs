using System.Management.Automation;
using Microsoft.PowerShell.Commands;

namespace Transform
{
  namespace Commands
  {
    [Cmdlet(
      VerbsCommon.Copy,
      "Guid"
    )]
    [OutputType(typeof(string))]
    public class CopyGuid : Cmdlet
    {
      [Parameter(
        HelpMessage = "Uppercase GUID"
      )]
      [Alias("Case")]
      public SwitchParameter Uppercase;

      [Parameter(
        HelpMessage = "Only copy GUID to clipboard, omit console output"
      )]
      public SwitchParameter Silent;

      protected override void EndProcessing()
      {
        string guid = System.Guid.NewGuid().ToString("D");

        if (Uppercase.IsPresent)
        {
          guid = guid.ToUpper();
        }

        if (!Silent.IsPresent)
        {
          WriteObject(guid);
        }

        Microsoft.PowerShell.Commands.SetClipboardCommand clip = new();
        clip.Value = guid;

        clip.Invoke();
      }
    }
  }
} // namespace Transform
