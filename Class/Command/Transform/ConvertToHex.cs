namespace Module.Command.Transform;

[Cmdlet(
  VerbsData.ConvertTo,
  "Hex",
  HelpUri = "https://learn.microsoft.com/dotnet/standard/base-types/composite-formatting"
)]
[Alias("hex")]
[OutputType(typeof(string))]
public class ConvertToHex : Cmdlet
{
  [Parameter(
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "Integer(s) to convert to hexadecimal"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long[] Number
  {
    get => numbers;
    set => numbers = value;
  }
  private long[] numbers = [];

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
    foreach (long number in numbers)
    {
      string hex = number.ToString("X");

      WriteObject(
        lowercase
          ? hex.ToLower()
          : hex
      );
    }
  }
}
