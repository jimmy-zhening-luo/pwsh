namespace Module.Command.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = "Path",
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[Alias("o", "open")]
[OutputType(typeof(void))]
public sealed class OpenUrl() : CoreCommand(
  true
)
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

  private protected sealed override void ProcessRecordAction()
  {
    if (ParameterSetName != "Uri")
    {
      return;
    }

    foreach (Uri uri in uris)
    {
      string url = uri
        .ToString()
        .Trim();

      if (!string.IsNullOrEmpty(url))
      {
        Invocation.ShellExecute(
          Application.Chrome,
          url
        );
      }
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    if (ParameterSetName != "Path")
    {
      return;
    }

    string cleanPath = FileSystem.Path.Normalizer.Normalize(
      path
    );
    string target = string.Empty;

    if (!string.IsNullOrEmpty(cleanPath))
    {
      string relativePath = IO.Path.GetRelativePath(
        SessionState.Path.CurrentLocation.Path,
        cleanPath
      );
      string testPath = IO.Path.IsPathRooted(
        relativePath
      )
        ? relativePath
        : IO.Path.Combine(
            SessionState.Path.CurrentLocation.Path,
            relativePath
          );

      target = IO.Path.Exists(
        testPath
      )
        ? IO.Path.GetFullPath(
            testPath
          )
        : cleanPath;
    }

    Invocation.ShellExecute(
      Application.Chrome,
      target
    );
  }
}
