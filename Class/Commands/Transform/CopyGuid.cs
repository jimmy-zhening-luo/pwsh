namespace Module.Commands.Transform;

[Cmdlet(
  VerbsCommon.Copy,
  "Guid",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097130"
)]
[Alias("gu", "guid")]
[OutputType(typeof(string))]
sealed public class CopyGuid : CoreCommand
{
  [Parameter(
    HelpMessage = "Uppercase GUID"
  )]
  [Alias("Case")]
  public SwitchParameter Uppercase
  {
    private get;
    set;
  }

  [Parameter(
    HelpMessage = "Only copy GUID to clipboard, omit console output"
  )]
  public SwitchParameter Silent
  {
    private get;
    set;
  }

  sealed override private protected void Postprocess()
  {
    var guid = System.Guid
      .NewGuid()
      .ToString("D");

    if (Uppercase)
    {
      guid = guid.ToUpper();
    }

    if (!Silent)
    {
      WriteObject(guid);
    }

    _ = AddCommand(
      @"Microsoft.PowerShell.Management\Set-Clipboard"
    )
      .AddParameter(
        "Value",
        new string[]
        {
          guid,
        }
      )
      .Invoke();
  }
}
