using System.Management.Automation;

namespace Transform
{
  namespace Commands
  {
    [Cmdlet(
      VerbsData.ConvertTo,
      "Hex"
    )]
    [OutputType(typeof(string[]))]
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
      [Alias("Integer")]
      [ValidateRange(0, long.MaxValue)]
      public long[] Number
      {
        get => numbers;
        set => numbers = value;
      }
      private long[] numbers;

      [Parameter(
        HelpMessage = "Output hexadecimal letters in lowercase"
      )]
      [Alias("Case")]
      public SwitchParameter Lowercase;

      protected override void ProcessRecord()
      {
        foreach (long number in numbers)
        {
          string hex = number.ToString("X");

          WriteObject(
            Lowercase
              ? hex.ToLower()
              : hex
          );
        }
      }
    }
  }
}
