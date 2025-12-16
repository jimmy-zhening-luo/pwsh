using System;
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
      public UInt32[] Number
      {
        get => numbers;
        set => numbers = value;
      }
      private UInt32[] numbers;

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
        foreach (UInt32 number in numbers)
        {
          string hex = number.ToString("X");
          string print = lowercase ? hex.ToLower() : hex;

          WriteObject(print, true);
        }
      }
    }
  }
}
