namespace PowerModule.Commands.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Url",
  HelpUri = $"{HelpLink}2097126"
)]
[Alias("tu")]
[OutputType(typeof(System.Uri))]
sealed public class TestUrl : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromRemainingArguments = true
  )]
  [AllowEmptyCollection]
  required public System.Uri[] Uri
  { get; init; }

  sealed override private protected void Process()
  {
    foreach (var uri in Uri)
    {
      switch (uri.Scheme)
      {
        case Client.Network.Scheme.File:
          if (System.IO.Path.Exists(uri.LocalPath))
          {
            WriteObject(uri);
          }

          break;

        case Client.Network.Scheme.Http:
        case Client.Network.Scheme.Https:
          var testUri = uri.IsAbsoluteUri
            ? uri
            : Client.Network.Url.ToAbsoluteHttpUri(
              $"http://{uri.OriginalString}"
            );

          if (
            testUri
            is not null
            && Client.Network.Dns.Resolve(
              testUri
            )
            && Client.Network.Url.TestHttp(
              testUri
            )
          )
          {
            WriteObject(testUri);
          }

          break;

        default:
          break;
      }
    }
  }
}
