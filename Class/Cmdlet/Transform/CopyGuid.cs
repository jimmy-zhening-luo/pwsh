using System.Management.Automation;

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
      
        Set-Clipboard -Value $Guid
      
        if (!Silent.IsPresent)
        {
          WriteObject(guid);
        }
      }
    }
  }
} // namespace Transform
