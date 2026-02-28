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
    get => pathUri?.ToString() ?? string.Empty;
    set
    {
      pathUri = default;

      if (!string.IsNullOrWhiteSpace(value))
      {
        if (Client.Network.Url.ToAbsoluteHttpUri(value) is { } httpUri)
        {
          pathUri = httpUri;
        }
        else if (
          Client.Network.Url.ToAbsoluteFileUri(value) is { } fileUri
          && System.IO.File.Exists(fileUri.LocalPath)
        )
        {
          pathUri = fileUri;
        }
        else if (
          Client.Network.Url.ToAbsoluteFileUri(
            Pwd(value)
          ) is { } relativeFileUri
          && System.IO.File.Exists(
            relativeFileUri.LocalPath
          )
        )
        {
          pathUri = relativeFileUri;
        }
        else if (
          System.Uri.TryCreate(
            value,
            System.UriKind.Relative,
            out var relativeHttpUri
          )
        )
        {
          pathUri = relativeHttpUri;
        }
      }
    }
  }
  private System.Uri? pathUri;

  [Parameter(
    ParameterSetName = "Uri",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The URL(s) to open."
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
          || Client.Network.Url.IsFile(uri)
          && System.IO.File.Exists(uri.LocalPath)
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

        case
        {
          IsAbsoluteUri: false,
          OriginalString: var s
        }
          when Client.Network.Url.ToAbsoluteHttpUri(
            $"http://{s}"
          ) is { } uri
          && Client.Network.Dns.Resolve(uri)
          && Client.Network.Url.Test(uri):
          Client.Network.Url.Open(uri);
          break;
      }
    }
  }
}
