using System;
using System.Linq;
using System.Management.Automation;

namespace Transform
{
  namespace Commands
  {
    [Cmdlet(VerbsData.ConvertTo, "Hex")]
    [OutputType(typeof(string[]))]
    public class ConvertToHex : Cmdlet
    {
      [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        ValueFromRemainingArguments = true
      )]
      [AllowEmptyCollection]
      [Alias("Integer")]
      public long[] Number
      {
        get => numbers;
        set => numbers = value;
      }
      private long[] numbers;

      [Parameter]
      [Alias("Case")]
      public SwitchParameter Lowercase
      {
        get => lowercase;
        set => lowercase = value;
      }
      private bool lowercase;

      protected override void ProcessRecord()
      {
        foreach (
          long number in numbers.Where(
            n => n >= 0
          )
        )
        {
          string hex = number.ToString("X");
          string print = lowercase ? hex.ToLower() : hex;

          WriteObject(print, true);
        }
      }
    }
  }
}
