namespace Module.Client.Network;

internal static class Url
{
  [System.Flags]
  internal enum Scheme
  {
    None,
    Http,
    File,
    HttpOrFile,
  }

  private static System.Net.Http.HttpClient? client;

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
    IsFile: true,
    LocalPath: var localPath
  }
    && !string.IsNullOrWhiteSpace(localPath);

  internal static bool IsHttpOrFile(
    [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
    System.Uri? uri
  ) => IsHttp(uri) || IsFile(uri);

  internal static System.Uri? ToAbsoluteHttpOrFileUri(object? uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);
  internal static System.Uri? ToAbsoluteHttpOrFileUri(string? uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);
  internal static System.Uri? ToAbsoluteHttpOrFileUri(System.Uri? uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);

  internal static System.Uri? ToAbsoluteFileUri(object? uri) => ToAbsoluteUri(uri, Scheme.File);
  internal static System.Uri? ToAbsoluteFileUri(string? uri) => ToAbsoluteUri(uri, Scheme.File);
  internal static System.Uri? ToAbsoluteFileUri(System.Uri? uri) => ToAbsoluteUri(uri, Scheme.File);

  internal static System.Uri? ToAbsoluteHttpUri(object? uri) => ToAbsoluteUri(uri);
  internal static System.Uri? ToAbsoluteHttpUri(string? uri) => ToAbsoluteUri(uri);
  internal static System.Uri? ToAbsoluteHttpUri(System.Uri? uri) => ToAbsoluteUri(uri);

  internal static System.Uri? ToAbsoluteUri(
    object? uri,
    Scheme scheme = Scheme.Http
  ) => ToAbsoluteUri(uri?.ToString(), scheme);
  internal static System.Uri? ToAbsoluteUri(
    string? uri,
    Scheme scheme = Scheme.Http
  ) => !string.IsNullOrWhiteSpace(uri)
    && System.Uri.TryCreate(
        uri,
        System.UriKind.Absolute,
        out var url
      )
    ? ToAbsoluteUri(url, scheme)
    : default;
  internal static System.Uri? ToAbsoluteUri(
    System.Uri? uri,
    Scheme scheme = Scheme.Http
  ) => (scheme | Scheme.Http) != default
    && IsHttp(uri)
    || (scheme | Scheme.File) != default
    && IsFile(uri)
    ? uri
    : default;

  internal static bool Test(System.Uri url)
  {
    if (!IsHttp(url))
    {
      return default;
    }

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
    if (
      IsHttp(uri)
      || IsFile(uri)
      && System.IO.File.Exists(uri.LocalPath)
    )
    {
      Open(uri.ToString());
    }
  }
  internal static void Open(string target)
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
