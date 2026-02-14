namespace Module.Command.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Url",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
)]
[Alias("tu")]
[OutputType(typeof(Uri))]
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
  public Uri[] Uri
  {
    get => uris;
    set => uris = value;
  }
  private Uri[] uris = [];

  private protected sealed override void ProcessRecordAction()
  {
    using var client = new System.Net.Http.HttpClient();

    foreach (Uri uri in uris)
    {
      Uri url = new(
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
            : "http://" + uri.OriginalString.Trim()
      );

      if (
        !string.IsNullOrEmpty(
          url.OriginalString
        )
        && Network.Url.HostExists(
          url.Host
        )
        && Network.Url.Test(
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
