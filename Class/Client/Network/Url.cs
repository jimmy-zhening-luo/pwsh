namespace Module.Client.Network;

internal static class Url
{
  static System.Net.Http.HttpClient? client;

  internal static bool IsHttp(
    [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
    System.Uri? uri
  ) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "http" or "https",
  };

  internal static bool IsFile(
    [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
    System.Uri? uri
  ) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "file",
  };

  internal static bool IsHttpOrFile(
    [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
    System.Uri? uri
  ) => IsHttp(uri) || IsFile(uri);

  internal static System.Uri? ToAbsoluteUri(object? uri) => ToAbsoluteUri(uri?.ToString());
  internal static System.Uri? ToAbsoluteUri(
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
    ? ToAbsoluteUri(url)
    : null;
  internal static System.Uri? ToAbsoluteUri(System.Uri? uri) => uri switch
  {
    null => null,
    {
      IsAbsoluteUri: true,
      Scheme: "http" or "https" or "file",
    } => uri,
    {
      IsAbsoluteUri: false,
      OriginalString: string s
    } when !string.IsNullOrWhiteSpace(s)
      && System.Uri.TryCreate(
          "http://" + s.Trim(),
          System.UriKind.Absolute,
          out var absoluteUrl
        ) => absoluteUrl,
    _ => null,
  };

  internal static bool Test(System.Uri url)
  {
    try
    {
      using var response = (
        client ??= new System.Net.Http.HttpClient(
          new System.Net.Http.SocketsHttpHandler()
          {
            PooledConnectionLifetime = System.TimeSpan.FromMinutes(2),
            ConnectTimeout = System.TimeSpan.FromMilliseconds(3000),
          }
        )
        {
          Timeout = System.TimeSpan.FromMilliseconds(3500),
        }
      )
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
    switch (uri)
    {
      case { IsAbsoluteUri: false }:
        break;

      case { Scheme: "http" or "https" }:
      case
      {
        Scheme: "file",
        LocalPath: string localPath
      } when System.IO.File.Exists(localPath):
        Open(uri.ToString());
        break;
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
