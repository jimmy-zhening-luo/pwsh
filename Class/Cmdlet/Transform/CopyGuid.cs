using System.Management.Automation;

namespace Transform
{
  namespace Commands
  {
    [Cmdlet(
      VerbsCommon.Copy,
      "Guid"
    )]
    [Alias("gu", "guid")]
    [OutputType(typeof(string))]
    public class CopyGuid : PSCmdlet
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

        using var ps = PowerShell.Create(
          RunspaceMode.CurrentRunspace
        );
        ps
          .AddCommand("Set-Clipboard")
          .AddParameter(
            "Value",
            new string[] { guid }
          );

        ps.Invoke();

        if (ps.HadErrors)
        {
          WriteError(
            new ErrorRecord(
              ps.Streams.Error[0].Exception,
              "Set-Clipboard Error",
              ErrorCategory.NotSpecified,
              null
            )
          );
        }

        if (!Silent.IsPresent)
        {
          WriteObject(guid);
        }
      }
    }
  }
} // namespace Transform
