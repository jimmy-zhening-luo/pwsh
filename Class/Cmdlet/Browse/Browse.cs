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
        browser = new();
        browser.StartInfo.FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
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

      private Process browser;

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
            }
          }
        }
      }

      protected override void EndProcessing()
      {
        if (Ssh() && ParameterSetName == "Path")
        {
          string cleanPath = path.Trim();

          if (cleanPath == string.Empty)
          {
            browser.StartInfo.Arguments = string.Empty;
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

            browser.StartInfo.Arguments = System.IO.Path.Exists(testPath)
              ? System.IO.Path.GetFullPath(
                  testPath
                )
              : cleanPath;
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
