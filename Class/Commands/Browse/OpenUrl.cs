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
    HelpMessage = "File path or URL to open, defaulting to the current directory"
  )]
  [Tab.Path.PathCompletions]
  public string Path
  {
    private get => pathUri?.ToString() ?? string.Empty;
    set => pathUri = value switch
    {
      "" => default,
      var path when Client.Network.Url.ToAbsoluteHttpUri(path) is { } url => url,
      var path when Client.Network.Url.ToAbsoluteFileUri(path) is { } fileUri && Client.Network.Url.TestFile(fileUri) => fileUri,
      var path => System.Uri.TryCreate(
        path,
        System.UriKind.Relative,
        out var relativeUrl
      )
        ? relativeUrl
        : default,
    };
  }
  private System.Uri? pathUri;

  [Parameter(
    ParameterSetName = "Uri",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "URLs to open"
  )]
  [AllowEmptyCollection]
  public System.Uri[] Uri
  {
    get => [.. uris];
    set
    {
      uris.Clear();

      foreach (var uri in value)
      {
        if (
          Client.Network.Url.IsHttp(uri)
          || Client.Network.Url.TestFile(uri)
        )
        {
          uris.Add(uri);
        }
      }
    }
  }
  private readonly List<System.Uri> uris = [];

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
      switch (pathUri)
      {
        case null:
          Client.Network.Url.Open();
          break;

        case { IsAbsoluteUri: true }:
          Client.Network.Url.Open(pathUri);
          break;

        case { OriginalString: var s }
          when Client.Network.Url.ToAbsoluteHttpUri(
            $"http://{s}"
          ) is { } url
          && Client.Network.Dns.Resolve(url)
          && Client.Network.Url.TestHttp(url):
          Client.Network.Url.Open(url);
          break;
      }
    }
  }
}
