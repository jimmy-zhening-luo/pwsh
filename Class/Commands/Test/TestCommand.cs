namespace Module.Commands.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Guid"
)]
[Alias("tc")]
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

  private protected sealed override void AfterEndProcessing()
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

    AddCommand(
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
