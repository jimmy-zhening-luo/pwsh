namespace Module.Commands.Browse.Test.Url;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Url",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
)]
[Alias("tu")]
[OutputType(typeof(System.Uri))]
public sealed class TestUrl : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "The URL to test. If the URL has no scheme, it defaults to 'http'."
  )]
  [AllowEmptyCollection]
  public System.Uri[] Uri
  {
    get => [.. urls];
    set
    {
      urls.Clear();

      foreach (var uri in value)
      {
        if (Client.Network.Url.IsHttpOrFile(uri))
        {
          urls.Add(uri);
        }
        else if (
          !uri.IsAbsoluteUri
          && Client.Network.Url.ToAbsoluteHttpUri(
            $"http://{uri.OriginalString.Trim()}"
          ) is { } httpUri
        )
        {
          urls.Add(httpUri);
        }
      }
    }
  }
  private readonly List<System.Uri> urls = [];

  private protected sealed override void Process()
  {
    foreach (var url in Uri)
    {
      if (
        Client.Network.Url.IsFile(url)
        && System.IO.Path.Exists(url.LocalPath)
        || Client.Network.Url.IsHttp(url)
        && Client.Network.Dns.Resolve(url)
        && Client.Network.Url.Test(url)
      )
      {
        WriteObject(url);
      }
    }
  }
}
