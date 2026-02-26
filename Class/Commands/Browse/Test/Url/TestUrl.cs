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
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "The URL to test. If the URL has no scheme, it defaults to 'http'."
  )]
  [AllowEmptyCollection]
  public System.Uri[] Uri
  { get; set; } = [];

  private protected sealed override void Processor()
  {
    using System.Net.Http.HttpClient client = new()
    {
      Timeout = System.TimeSpan.FromMilliseconds(
        3500
      )
    };

    foreach (var uri in Uri)
    {
      System.Uri url = new(
        uri.IsAbsoluteUri
          ? string.IsNullOrEmpty(
              uri.Host.Trim()
            )
            ? string.Empty
            : uri.AbsoluteUri.Trim()
          : string.IsNullOrEmpty(
              uri.OriginalString.Trim()
            )
            ? string.Empty
            : "http://"
              + uri.OriginalString.Trim()
      );

      if (
        !string.IsNullOrEmpty(
          url.OriginalString
        )
        && Client.Network.Url.HostExists(
          url.Host
        )
        && Client.Network.Url.Test(
          client,
          url
        )
      )
      {
        WriteObject(
          url
        );
      }
    }
  }
}
