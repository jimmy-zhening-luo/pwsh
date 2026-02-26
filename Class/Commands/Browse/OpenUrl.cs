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
    Position = default,
    HelpMessage = "The file path or URL to open. Defaults to the current directory."
  )]
  [PathCompletions]
  public string Path
  {
    get => path;
    set => path = Client.FileSystem.PathString.Normalize(
      value
    );
  }
  private string path = string.Empty;

  [Parameter(
    ParameterSetName = "Uri",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The URL(s) to open."
  )]
  [AllowEmptyCollection]
  public System.Uri[] Uri { get; set; } = [];

  private protected sealed override void Processor()
  {
    foreach (var uri in Uri)
    {
      Client.Network.Url.Open(
        uri
      );
    }
  }

  private protected sealed override void Postprocess()
  {
    if (ParameterSetName is "Path")
    {
      var target = string.Empty;

      if (Path is not "")
      {
        var relativePath = System.IO.Path.GetRelativePath(
          SessionState.Path.CurrentLocation.Path,
          Path
        );
        var testPath = System.IO.Path.IsPathRooted(
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
          : Path;
      }

      Client.Network.Url.Open(
        target
      );
    }
  }
}
