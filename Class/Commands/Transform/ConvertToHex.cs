namespace PowerModule.Commands.Transform;

[Cmdlet(
  VerbsData.ConvertTo,
  "Hex",
  HelpUri = "https://learn.microsoft.com/dotnet/standard/base-types/composite-formatting"
)]
[Alias("hex")]
[OutputType(typeof(string))]
sealed public class ConvertToHex : Cmdlet
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "Integers to convert"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  required public long[] Number
  { get; init; }

  [Parameter(
    HelpMessage = "Output in lowercase"
  )]
  [Alias("Case")]
  public SwitchParameter Lowercase
  { private get; init; }

  sealed override protected void ProcessRecord()
  {
    foreach (var number in Number)
    {
      var hex = number.ToString(
        "X",
        Client.StringInput.CurrentCulture
      );

      WriteObject(
        Lowercase
          ? hex.ToLower(
            Client.StringInput.CurrentCulture
          )
          : hex
      );
    }
  }
}
