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
    public class OpenUrl : PSCmdlet
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
      private Uri[] uri = [];

      protected override void ProcessRecord()
      {
        if (Ssh() && ParameterSetName == "Uri")
        {
          foreach (Uri u in uri)
          {
            string url = u.ToString().Trim();

            if (url != string.Empty)
            {
              Process browser = new();
              browser.StartInfo = new ProcessStartInfo(
                Browser,
                url
              );

              browser.Start();
            }
          }
        }
      }

      protected override void EndProcessing()
      {
        if (Ssh() && ParameterSetName == "Path")
        {
          string cleanPath = path.Trim();
          Process browser = new();

          if (cleanPath == string.Empty)
          {
            browser.StartInfo = new ProcessStartInfo(Browser);
          }
          else
          {
            string relativePath = System.IO.Path.GetRelativePath(
              Pwd(),
              cleanPath
            );
            string testPath = System.IO.Path.IsPathRooted(
              relativePath
            )
              ? relativePath
              : System.IO.Path.Combine(
                  Pwd(),
                  relativePath
                );

            if (System.IO.Path.Exists(testPath))
            {
              browser.StartInfo = new ProcessStartInfo(
                Browser,
                System.IO.Path.GetFullPath(
                  relativePath
                )
              );
            }
            else
            {
              browser.StartInfo = new ProcessStartInfo(Browser);
            }
          }

          browser.Start();
        }
      }

      private bool Ssh() => Environment.GetEnvironmentVariable(
        "SSH_CLIENT"
      ) == null;

      private string Pwd() => this
        .SessionState
        .Path
        .CurrentLocation
        .Path;
    }
  }
} // namespace Browse
