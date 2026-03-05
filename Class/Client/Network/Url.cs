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

  private static System.Net.Http.HttpClient HttpClient => client ??= new System.Net.Http.HttpClient(
    new System.Net.Http.SocketsHttpHandler()
    {
      PooledConnectionLifetime = System.TimeSpan.FromMinutes(2),
      ConnectTimeout = System.TimeSpan.FromMilliseconds(3000),
    }
  )
  {
    Timeout = System.TimeSpan.FromMilliseconds(3500),
  };
  private static System.Net.Http.HttpClient? client;

  internal static bool IsHttp(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "http" or "https",
  };

  internal static bool IsFile(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "file",
    IsFile: true,
    LocalPath: { } localPath,
  }
    && localPath is not "";

  internal static bool IsHttpOrFile(System.Uri uri) => IsHttp(uri) || IsFile(uri);

  internal static System.Uri? ToAbsoluteHttpUri(string uri) => ToAbsoluteUri(uri);
  internal static System.Uri? ToAbsoluteHttpUri(System.Uri uri) => ToAbsoluteUri(uri);

  internal static System.Uri? ToAbsoluteFileUri(string uri) => ToAbsoluteUri(uri, Scheme.File);
  internal static System.Uri? ToAbsoluteFileUri(System.Uri uri) => ToAbsoluteUri(uri, Scheme.File);

  internal static System.Uri? ToAbsoluteHttpOrFileUri(string uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);
  internal static System.Uri? ToAbsoluteHttpOrFileUri(System.Uri uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);

  internal static System.Uri? ToAbsoluteUri(
    string uri,
    Scheme scheme = Scheme.Http
  ) => uri is not ""
    && System.Uri.TryCreate(
        uri,
        System.UriKind.Absolute,
        out var url
      )
    ? ToAbsoluteUri(url, scheme)
    : default;
  internal static System.Uri? ToAbsoluteUri(
    System.Uri uri,
    Scheme scheme = Scheme.Http
  ) => (scheme | Scheme.Http) != default
    && IsHttp(uri)
    || (scheme | Scheme.File) != default
    && IsFile(uri)
    ? uri
    : default;

  internal static bool TestHttp(System.Uri uri)
  {
    if (!IsHttp(uri))
    {
      return default;
    }

    try
    {
      using var response = HttpClient
        .GetAsync(
          uri,
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

  internal static bool TestFile(System.Uri uri) => IsFile(uri)
    && System.IO.Path.Exists(uri.LocalPath);

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
