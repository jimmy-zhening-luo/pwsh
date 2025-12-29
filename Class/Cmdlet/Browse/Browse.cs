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

      private static bool Ssh() => Environment.GetEnvironmentVariable(
        "SSH_CLIENT"
      ) == null;

      protected override void ProcessRecord()
      {
        if (Ssh() && ParameterSetName == "Uri")
        {
          Process browser = new();
          browser.StartInfo.FileName = Browser;

          foreach (Uri u in uri)
          {
            string url = u.ToString().Trim();

            if (url != string.Empty)
            {
              browser.StartInfo.Arguments = url;

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
          browser.StartInfo.FileName = Browser;

          if (cleanPath != string.Empty)
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
              browser.StartInfo.Arguments = System.IO.Path.GetFullPath(
                testPath
              );
            }
          }

          browser.Start();
        }
      }

      private string Pwd() => this
        .SessionState
        .Path
        .CurrentLocation
        .Path;
    }
  }
} // namespace Browse
