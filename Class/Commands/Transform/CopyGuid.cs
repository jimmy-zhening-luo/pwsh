namespace PowerModule.Commands.Transform;

[Cmdlet(
  VerbsCommon.Copy,
  "Guid",
  HelpUri = $"{HelpLink}2097130"
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
  { private get; init; }

  [Parameter(
    HelpMessage = "Only copy GUID to clipboard, omit console output"
  )]
  public SwitchParameter Silent
  { private get; init; }

  sealed override private protected void Postprocess()
  {
    var guid = System.Guid
      .NewGuid()
      .ToString("D");

    if (Uppercase)
    {
      guid = guid.ToUpper(
        Client.StringInput.CurrentCulture
      );
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
        guid
      )
      .Invoke();
  }
}
