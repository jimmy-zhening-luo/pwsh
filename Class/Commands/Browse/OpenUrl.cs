namespace Module.Commands.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = "Path",
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[Alias("o", "open")]
[OutputType(typeof(void))]
public sealed class OpenUrl() : CoreCommand(true)
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
    set => path = Client.File.PathString.Normalize(value);
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

  private protected sealed override void Process()
  {
    foreach (var uri in Uri)
    {
      Client.Network.Url.Open(uri);
    }
  }

  private protected sealed override void Postprocess()
  {
    if (ParameterSetName is "Path")
    {
      var target = string.Empty;

      if (Path is not "")
      {
        var fullPath = Pwd(path);

        target = System.IO.Path.Exists(fullPath)
          ? fullpath
          : Path;
      }

      Client.Network.Url.Open(target);
    }
  }
}
