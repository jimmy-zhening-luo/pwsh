using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;

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
      public UInt64[] Number
      {
        get => numbers;
        set => numbers = value;
      }
      private UInt64[] numbers;

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
        foreach (UInt64 number in numbers)
        {
          string hex = number.ToString("X");
          string print = lowercase ? hex.ToLower() : hex;

          WriteObject(print, true);
        }
      }
    }
  }
}
