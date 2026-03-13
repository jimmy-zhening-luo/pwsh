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
  static List<System.Uri> FilterSupportedUri(System.Uri[] uris)
  {
    List<System.Uri> supportedUris = [];

    foreach (var uri in uris)
    {
      if (Client.Network.Url.IsHttpOrFile(uri))
      {
        supportedUris.Add(uri);
      }
      else if (
        !uri.IsAbsoluteUri
        && Client.Network.Url.ToAbsoluteHttpUri(
          $"http://{uri.OriginalString}"
        ) is { } httpUri
      )
      {
        supportedUris.Add(httpUri);
      }
    }

    return supportedUris;
  }

  static List<System.Uri> FilterReachableUri(List<System.Uri> uris)
  {
    List<System.Uri> reachableUris = [];

    foreach (var uri in uris)
    {
      if (
        Client.Network.Url.IsFile(uri)
        && System.IO.Path.Exists(uri.LocalPath)
        || Client.Network.Url.IsHttp(uri)
        && Client.Network.Dns.Resolve(uri)
        && Client.Network.Url.TestHttp(uri)
      )
      {
        reachableUris.Add(uri);
      }
    }

    return reachableUris;
  }

  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "URL to test"
  )]
  [AllowEmptyCollection]
  [ValidateNotNull]
  required public System.Uri[] Uri
  { get; init; }

  sealed override private protected void Process() => WriteObject(
    FilterReachableUri(
      FilterSupportedUri(
        Uri
      )
    ),
    true
  );
}
