namespace Module.Command.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = "Path",
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[Alias("o", "open")]
[OutputType(typeof(void))]
public sealed class OpenUrl : CoreCommand
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    HelpMessage = "The file path or URL to open. Defaults to the current directory."
  )]
  [PathCompletions]
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

  private protected sealed override bool NoSsh => true;

  private protected sealed override void ProcessRecordAction()
  {
    if (ParameterSetName == "Uri")
    {
      foreach (Uri uri in uris)
      {
        string url = uri
          .ToString()
          .Trim();

        if (!string.IsNullOrEmpty(url))
        {
          Invocation.ShellExecute(
            @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            url
          );
        }
      }
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    if (ParameterSetName == "Path")
    {
      string cleanPath = Normalize(
        path
      );
      string target = string.Empty;

      if (!string.IsNullOrEmpty(cleanPath))
      {
        string relativePath = GetRelativePath(
          SessionState.Path.CurrentLocation.Path,
          cleanPath
        );
        string testPath = IsPathRooted(
          relativePath
        )
          ? relativePath
          : Combine(
              SessionState.Path.CurrentLocation.Path,
              relativePath
            );

        target = Exists(
          testPath
        )
          ? GetFullPath(
              testPath
            )
          : cleanPath;
      }

      Invocation.ShellExecute(
        @"C:\Program Files\Google\Chrome\Application\chrome.exe",
        target
      );
    }
  }
}
