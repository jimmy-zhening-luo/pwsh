using System;
using System.Diagnostics;
using System.Management.Automation;
using Completer.PathCompleter;

namespace Core.Browse.Commands
{
  [Cmdlet(
    VerbsCommon.Open,
    "Url",
    DefaultParameterSetName = "Path",
    HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
  )]
  [OutputType(typeof(void))]
  [Alias("o", "open")]
  public class OpenUrl : PSCmdlet
  {
    [Parameter(
      ParameterSetName = "Path",
      Position = 0,
      HelpMessage = "The file path or URL to open. Defaults to the current directory."
    )]
    [AllowEmptyString]
    [RelativePathCompletions]
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

    protected override void ProcessRecord()
    {
      if (!Context.Ssh() && ParameterSetName == "Uri")
      {
        foreach (Uri u in uri)
        {
          ProcessStartInfo psi = new(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
          string url = u.ToString().Trim();

          if (url != string.Empty)
          {
            psi.Arguments = url;
            Process.Start(psi);
          }
        }
      }
    }

    protected override void EndProcessing()
    {
      if (!Context.Ssh() && ParameterSetName == "Path")
      {
        ProcessStartInfo psi = new(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
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

          psi.Arguments = System.IO.Path.Exists(testPath)
            ? System.IO.Path.GetFullPath(
                testPath
              )
            : cleanPath;
        }

        Process.Start(psi);
      }
    }
  }
}
