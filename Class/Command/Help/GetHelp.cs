namespace Module.Command.Help;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096483"
)]
[Alias("man", "m")]
[OutputType(typeof(object))]
public sealed class GetHelpOnline : CoreCommand
{
  [Parameter(
    Position = 0,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [Completions("*")]
  public string[] Name
  {
    get => names;
    set => names = value;
  }
  private string[] names = [];

  [Parameter]
  public string[] Parameter
  {
    get => parameters;
    set => parameters = value;
  }
  private string[] parameters = [];

  private protected sealed override bool ValidateParameters() => names.Length == 0
    || names.Length == 1
    && string.IsNullOrEmpty(
      names[0]
    );

  private protected sealed override void DefaultAction()
  {
    WriteObject("Hello");
    WriteObject(
      AddCommand(
        "Get-Help"
      )
        .AddParameter(
          "Name",
          "Get-Help"
        )
        .Invoke(),
      true
    );
  }
}
