namespace Module.Commands.Transform;

[Cmdlet(
  VerbsCommon.Copy,
  "Guid",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097130"
)]
[Alias("gu", "guid")]
[OutputType(typeof(string))]
public sealed class CopyGuid : CoreCommand
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

  private protected sealed override void Postprocess()
  {
    string guid = System.Guid
      .NewGuid()
      .ToString(
        "D"
      );

    if (uppercase)
    {
      guid = guid.ToUpper();
    }

    if (!silent)
    {
      WriteObject(
        guid
      );
    }

    _ = AddCommand(
      "Set-Clipboard"
    )
      .AddParameter(
        "Value",
        new string[]
        {
          guid
        }
      )
      .Invoke();
  }
}
