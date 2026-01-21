namespace Module.Command.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = "Path",
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[OutputType(typeof(void))]
[Alias("o", "open")]
public class OpenUrl : CoreCommand
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
    get => uris;
    set => uris = value;
  }
  private Uri[] uris = [];

  protected override void ProcessRecord()
  {
    if (ParameterSetName == "Uri")
    {
      foreach (Uri uri in uris)
      {
        string url = uri.ToString().Trim();

        if (!string.IsNullOrEmpty(url))
        {
          ShellExecute(
            @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            url
          );
        }
      }
    }
  }

  protected override void EndProcessing()
  {
    if (ParameterSetName == "Path")
    {
      string cleanPath = path.Trim();
      string target = string.Empty;

      if (!string.IsNullOrEmpty(cleanPath))
      {
        string relativePath = global::Path.GetRelativePath(
          SessionState.Path.CurrentLocation.Path,
          cleanPath
        );
        string testPath = global::Path.IsPathRooted(
          relativePath
        )
          ? relativePath
          : global::Path.Combine(
              SessionState.Path.CurrentLocation.Path,
              relativePath
            );

        target = global::Path.Exists(testPath)
          ? global::Path.GetFullPath(
              testPath
            )
          : cleanPath;
      }

      ShellExecute(
        @"C:\Program Files\Google\Chrome\Application\chrome.exe",
        target
      );
    }
  }
}
