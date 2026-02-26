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
        System.Uri url = new(
          uri.IsAbsoluteUri
            ? string.IsNullOrWhiteSpace(
                uri.Host
              )
              ? string.Empty
              : uri.AbsoluteUri.Trim()
            : string.IsNullOrWhiteSpace(
                uri.OriginalString
              )
              ? string.Empty
              : string.Concat(
                  "http://",
                  uri.OriginalString.Trim()
                )
        );
        if (
          url is not
          {
            OriginalString: ""
          }
        )
        {
          urls.Add(
            url
          );
        }
      }
    }
  }
  private readonly List<System.Uri> urls = [];

  private protected sealed override void Process()
  {
    using System.Net.Http.HttpClient client = new()
    {
      Timeout = System.TimeSpan.FromMilliseconds(
        3500
      )
    };

    foreach (var url in Uri)
    {
      if (
        Client.Network.Dns.Resolve(url)
        && Client.Network.Url.Test(
          url,
          client
        )
      )
      {
        WriteObject(url);
      }
    }
  }
}
