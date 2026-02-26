namespace Module.Commands.Transform;

[Cmdlet(
  VerbsData.ConvertTo,
  "Hex",
  HelpUri = "https://learn.microsoft.com/dotnet/standard/base-types/composite-formatting"
)]
[Alias("hex")]
[OutputType(typeof(string))]
public sealed class ConvertToHex : Cmdlet
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "Integer(s) to convert to hexadecimal"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long[] Number { get; set; } = [];

  [Parameter(
    HelpMessage = "Output hexadecimal letters in lowercase"
  )]
  [Alias("Case")]
  public SwitchParameter Lowercase
  {
    get => lowercase;
    set => lowercase = value;
  }
  private bool lowercase;

  protected sealed override void ProcessRecord()
  {
    foreach (var number in Number)
    {
      var hex = number.ToString("X");

      WriteObject(
        lowercase
          ? hex.ToLower()
          : hex
      );
    }
  }
}
