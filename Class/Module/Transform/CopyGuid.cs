namespace Module.Transform
{
  namespace Commands
  {
    using System.Management.Automation;

    [Cmdlet(
      VerbsCommon.Copy,
      "Guid",
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097130"
    )]
    [Alias("gu", "guid")]
    [OutputType(typeof(string))]
    public class CopyGuid : PSCoreCommand
    {
      [Parameter(
        HelpMessage = "Uppercase GUID"
      )]
      [Alias("Case")]
      public SwitchParameter Uppercase
      {
        get => uppercase;
        set => uppercase = value;
      }
      private bool uppercase;

      [Parameter(
        HelpMessage = "Only copy GUID to clipboard, omit console output"
      )]
      public SwitchParameter Silent
      {
        get => silent;
        set => silent = value;
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

        if (!silent)
        {
          WriteObject(guid);
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
      }
    }
  }
}
