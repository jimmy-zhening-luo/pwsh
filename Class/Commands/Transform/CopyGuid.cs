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
  [Parameter]
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
      $@"{StandardModule.Management}\Set-Clipboard"
    )
      .AddParameter(
        "Value",
        guid
      )
      .Invoke();
  }
}
