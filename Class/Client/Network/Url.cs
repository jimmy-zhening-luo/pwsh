namespace Module.Client.Network;

internal static class Url
{
  internal static System.Uri? ToAbsoluteUrl(object? uri) => ToAbsoluteUrl(uri?.ToString());
  internal static System.Uri? ToAbsoluteUrl(
    [System.Diagnostics.CodeAnalysis.StringSyntax(
      System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.Uri
    )]
    string? uri
  ) => !string.IsNullOrWhiteSpace(uri)
    && System.Uri.TryCreate(
        uri,
        System.UriKind.RelativeOrAbsolute,
        out var url
      )
    ? ToAbsoluteUrl(url)
    : null;
  internal static System.Uri? ToAbsoluteUrl(System.Uri? uri) => uri switch
  {
    null => null,
    {
      IsAbsoluteUri: true,
      Scheme: string scheme,
      Host: string host,
    } when string.IsNullOrWhiteSpace(host)
      || scheme is not ("http" or "https") => null,
    { IsAbsoluteUri: true } => uri,
    { OriginalString: string s } when !string.IsNullOrWhiteSpace(s)
      && System.Uri.TryCreate(
          "http://" + s.Trim(),
          System.UriKind.Absolute,
          out var absoluteUrl
        ) => absoluteUrl,
    _ => null,
  };

  internal static bool Test(
    System.Uri url,
    System.Net.Http.HttpClient client
  )
  {
    try
    {
      using var response = client
        .GetAsync(
          url,
          System.Net.Http.HttpCompletionOption.ResponseHeadersRead
        )
        .GetAwaiter()
        .GetResult();

      return response.IsSuccessStatusCode;
    }
    catch (System.Net.Http.HttpRequestException)
    {
      return default;
    }
    catch (System.InvalidOperationException)
    {
      return default;
    }
  }

  internal static void Open() => Open(string.Empty);
  internal static void Open(System.Uri uri)
  {
    if (
      uri.ToString() is string url
      && !string.IsNullOrWhiteSpace(url)
    )
    {
      Open(url);
    }
  }
  internal static void Open(
    [System.Diagnostics.CodeAnalysis.StringSyntax(
      System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.Uri
    )]
    string target
  )
  {
    if (!Environment.Known.Variable.InSsh)
    {
      Start.ShellExecute(
        Environment.Known.Application.Chrome,
        target
      );
    }
  }
}
