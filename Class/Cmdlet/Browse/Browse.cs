using System;
using System.Diagnostics;
using System.Management.Automation;

namespace Browse
{
  namespace Commands
  {
    [Cmdlet(
      VerbsCommon.Open,
      "Url",
      DefaultParameterSetName = "Path"
    )]
    [OutputType(typeof(void))]
    public class ConvertToHex : Cmdlet
    {
      private static Browser = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        HelpMessage = "The file path or URL to open. Defaults to the current directory."
      )]
      [AllowEmptyString]
      public string Path
      {
        get => path;
        set => path = value;
      }
      private string path;

      [Parameter(
        ParameterSetName = "Uri",
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        HelpMessage = "The URL(s) to open."
      )]
      [AllowEmptyCollection]
      public Uri[] Uri
      {
        get => uri;
        set => uri = value;
      }
      private Uri[] uri;

      protected override void ProcessRecord()
      {
        foreach (Uri u in uri)
        {
          Process browser = new ();
          browser.StartInfo = new ProcessStartInfo(
            Browser,
            u.ToString()
          );

          Process.Start();
        }
      }

      protected override void EndProcessing()
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
} // namespace Browse
