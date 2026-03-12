namespace PowerModule.Commands.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Url",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
)]
[Alias("tu")]
[OutputType(typeof(System.Uri))]
sealed public class TestUrl : CoreCommand
{
  static IEnumerable<System.Uri> EnumerateSupportedUri(
    IEnumerable<System.Uri> uris
  )
  {
    foreach (var uri in uris)
    {
      if (Client.Network.Url.IsHttpOrFile(uri))
      {
        yield return uri;
      }
      else if (
        !uri.IsAbsoluteUri
        && Client.Network.Url.ToAbsoluteHttpUri(
          $"http://{uri.OriginalString}"
        ) is { } httpUri
      )
      {
        yield return httpUri;
      }
    }

    yield break;
  }

  static IEnumerable<System.Uri> EnumerateReachableUri(
    IEnumerable<System.Uri> uris
  )
  {
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
        yield return uri;
      }
    }

    yield break;
  }

  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819: Properties should not return arrays", Justification = "PowerShell: Required to bind parameter values from remaining arguments as a list of values.")]
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
    EnumerateReachableUri(
      EnumerateSupportedUri(
        Uri
      )
    ),
    true
  );
}
