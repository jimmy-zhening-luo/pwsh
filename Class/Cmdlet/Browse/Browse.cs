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
    [Alias("o", "open")]
    public class OpenUrl : PSCmdlet
    {
      public OpenUrl() : base()
      {

      }

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
      private string path = string.Empty;

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

      private Process browser = new()
      {
        StartInfo = new()
        {
          FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
        }
      };

      private static bool Ssh() => Environment.GetEnvironmentVariable(
        "SSH_CLIENT"
      ) == null;

      protected override void ProcessRecord()
      {
        if (Ssh() && ParameterSetName == "Uri")
        {
          foreach (Uri u in uri)
          {
            string url = u.ToString().Trim();

            if (url != string.Empty)
            {
              browser.StartInfo.Arguments = url;
              browser.Start();
              browser.StartInfo.Arguments = string.Empty;
            }
          }
        }
      }

      protected override void EndProcessing()
      {
        if (Ssh() && ParameterSetName == "Path")
        {
          browser.StartInfo.Arguments = string.Empty;
          string cleanPath = path.Trim();

          if (cleanPath != string.Empty)
          {
            string relativePath = System.IO.Path.GetRelativePath(
              SessionState.Path.CurrentLocation.Path,
              cleanPath
            );
            string testPath = System.IO.Path.IsPathRooted(
              relativePath
            )
              ? relativePath
              : System.IO.Path.Combine(
                  SessionState.Path.CurrentLocation.Path,
                  relativePath
                );

            browser.StartInfo.Arguments = System.IO.Path.Exists(testPath)
              ? System.IO.Path.GetFullPath(
                  testPath
                )
              : cleanPath;
          }

          browser.Start();
          browser.StartInfo.Arguments = string.Empty;
        }
      }
    }
  }
} // namespace Browse
