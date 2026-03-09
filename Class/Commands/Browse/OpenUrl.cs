namespace PowerModule.Commands.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = "Path",
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[Alias("o", "open")]
[OutputType(typeof(void))]
sealed public class OpenUrl() : CoreCommand(true)
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    HelpMessage = "File path or URL to open, defaulting to the current directory"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  public string Path
  { private get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "Uri",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "URLs to open"
  )]
  [AllowEmptyCollection]
  [ValidateNotNull]
  public required System.Uri[] Uri
  { get; set; }

  sealed override private protected void Process()
  {
    foreach (var uri in Uri)
    {
      if (Client.Network.Url.IsHttpOrFile(uri))
      {
        Client.Network.Url.Open(uri);
      }
    }
  }

  sealed override private protected void Postprocess()
  {
    if (ParameterSetName is "Path")
    {
      switch (Path)
      {
        case "":
          Client.Network.Url.Open();
          break;

        case var path when Client.Network.Url.ToAbsoluteHttpUri(path) is { } url:
          Client.Network.Url.Open(url);
          break;

        case var path when Client.Network.Url.ToAbsoluteFileUri(path) is { } fileUri && Client.Network.Url.TestFile(fileUri):
          Client.Network.Url.Open(fileUri);
          break;

        case var path when Client.Network.Url.ToAbsoluteHttpUri(
            $"http://{path}"
          ) is { } url
          && Client.Network.Dns.Resolve(url)
          && Client.Network.Url.TestHttp(url):
          Client.Network.Url.Open(url);
          break;
      }
    }
  }
}
