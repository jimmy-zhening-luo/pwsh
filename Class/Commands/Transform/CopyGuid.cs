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
    HelpMessage = "Get Guid in uppercase"
  )]
  [Alias("Case")]
  public SwitchParameter Uppercase
  { private get; init; }

  sealed override private protected void Postprocess()
  {
    var guid = System.Guid
      .NewGuid()
      .ToString("D");

    if (Uppercase)
    {
      guid = guid.ToUpper(
        Client.StringInput.InvariantCulture
      );
    }

    WriteObject(guid);

    _ = AddCommand(
      @"Microsoft.PowerShell.Management\Set-Clipboard"
    )
      .AddParameter(
        StandardParameter.Value,
        guid
      )
      .Invoke();
  }
}
