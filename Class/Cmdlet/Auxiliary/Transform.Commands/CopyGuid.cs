namespace Auxiliary.Transform.Commands
{
  using System.Management.Automation;

  [Cmdlet(
    VerbsCommon.Copy,
    "Guid",
    HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097130"
  )]
  [Alias("gu", "guid")]
  [OutputType(typeof(string))]
  public class CopyGuid : PSCmdlet
  {
    [Parameter(
      HelpMessage = "Uppercase GUID"
    )]
    [Alias("Case")]
    public SwitchParameter Uppercase
    {
      get { return uppercase; }
      set { uppercase = value; }
    }
    private bool uppercase;

    [Parameter(
      HelpMessage = "Only copy GUID to clipboard, omit console output"
    )]
    public SwitchParameter Silent
    {
      get { return silent; }
      set { silent = value; }
    }
    private bool silent;

    [Parameter(DontShow = true)]
    public SwitchParameter z;

    protected override void EndProcessing()
    {
      string guid = System.Guid.NewGuid().ToString("D");

      if (uppercase)
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

      if (!silent)
      {
        WriteObject(guid);
      }
    }
  }
}
