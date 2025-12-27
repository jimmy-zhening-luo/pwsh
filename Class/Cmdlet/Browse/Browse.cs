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
    public class ConvertToHex : PSCmdlet
    {
      private static string Browser = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

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
        if (
          ParameterSetName == "Uri"
          && Environment.GetEnvironmentVariable("SSH_CLIENT") != null
        )
        {
          foreach (Uri u in uri)
          {
            string url = u.ToString().Trim();

            if (url != string.Empty)
            {
              Process browser = new ();
              browser.StartInfo = new ProcessStartInfo(
                Browser,
                u.ToString()
              );

              browser.Start();
            }
          }
        }
      }

      protected override void EndProcessing()
      {
        if (
          this.ParameterSetName == "Path"
          && Environment.GetEnvironmentVariable("SSH_CLIENT") != null
        )
        {
          string cleanPath = path.Trim();
          string pathUri = System.IO.Path.Exists(cleanPath)
            ? System.IO.Path.GetFullPath(cleanPath)
            : cleanPath;

          Process browser = new ();
          browser.StartInfo.FileName = cleanPath == string.Empty
            ? new ProcessStartInfo(Browser)
            : new ProcessStartInfo(
                Browser,
                pathUri
              );

          browser.Start();
        }
      }
    }
  }
} // namespace Browse
