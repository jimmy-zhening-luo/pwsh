namespace PowerModule.Commands.Browse;

[Cmdlet(
  VerbsCommon.Open,
  "Url",
  DefaultParameterSetName = nameof(Path),
  HelpUri = "https://www.chromium.org/developers/how-tos/run-chromium-with-flags/"
)]
[Alias("o", "open")]
[OutputType(typeof(void))]
sealed public class OpenUrl() : CoreCommand(true)
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    HelpMessage = "File path or URL to open"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  public string Path
  { private get; init; } = string.Empty;

  [Parameter(
    ParameterSetName = nameof(Uri),
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true
  )]
  [AllowEmptyCollection]
  required public System.Uri[] Uri
  { get; init; }

  sealed override private protected void Process()
  {
    if (ParameterSetName is nameof(Uri))
    {
      foreach (var uri in Uri)
      {
        if (Client.Network.Url.IsHttpOrFile(uri))
        {
          Client.Network.Url.Open(uri);
        }
      }
    }
  }

  sealed override private protected void Postprocess()
  {
    if (ParameterSetName is nameof(Path))
    {
      switch (Path)
      {
        case "":
          Client.Network.Url.Open();

          break;

        case var path when Client.Network.Url.ToAbsoluteHttpUri(path) is { } url:
          Client.Network.Url.Open(url);

          break;

        case var path when Client.Network.Url.ToAbsoluteFileUri(path) is { } fileUri:
          if (Client.Network.Url.TestFile(fileUri))
          {
            Client.Network.Url.Open(fileUri);
          }

          break;

        case var path when Client.Network.Url.ToAbsoluteHttpUri(
            $"http://{path}"
        ) is { } url
        && Client.Network.Dns.Resolve(url)
        && Client.Network.Url.TestHttp(url):
          Client.Network.Url.Open(url);

          break;

        default:
          break;
      }
    }
  }
}
