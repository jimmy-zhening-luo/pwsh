namespace Module.Commands.Browse;

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
  public string Path { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "Uri",
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The URL(s) to open."
  )]
  [AllowEmptyCollection]
  public System.Uri[] Uri { get; set; } = [];

  private protected sealed override void ProcessRecordAction()
  {
    foreach (var uri in Uri)
    {
      Client.Network.Url.Open(
        uri
      );
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    if (ParameterSetName != "Path")
    {
      return;
    }

    string cleanPath = Client.FileSystem.PathString.Normalize(
      Path
    );
    string target = string.Empty;

    if (
      !string.IsNullOrEmpty(
        cleanPath
      )
    )
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

      target = System.IO.Path.Exists(
        testPath
      )
        ? System.IO.Path.GetFullPath(
            testPath
          )
        : cleanPath;
    }

    Client.Network.Url.Open(
      target
    );
  }
}
