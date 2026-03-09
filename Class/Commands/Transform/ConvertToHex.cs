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
    HelpMessage = "Integer(s) to convert to hexadecimal"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  required public Collection<long> Number
  { get; init; }

  [Parameter(
    HelpMessage = "Output hexadecimal letters in lowercase"
  )]
  [Alias("Case")]
  public SwitchParameter Lowercase
  { private get; set; }

  sealed override protected void ProcessRecord()
  {
    foreach (var number in Number)
    {
      var hex = number.ToString(
        "X",
        Client.String.InvariantCulture
      );

      WriteObject(
        Lowercase
          ? hex.ToLower(
            Client.String.InvariantCulture
          )
          : hex
      );
    }
  }
}
