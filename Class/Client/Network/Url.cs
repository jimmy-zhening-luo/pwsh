namespace Module.Client.Network;

static internal class Url
{
  [System.Flags]
  internal enum Scheme
  {
    None,
    Http,
    File,
    HttpOrFile,
  }

  static private System.Net.Http.HttpClient HttpClient => client ??= new System.Net.Http.HttpClient(
    new System.Net.Http.SocketsHttpHandler()
    {
      PooledConnectionLifetime = System.TimeSpan.FromMinutes(2),
      ConnectTimeout = System.TimeSpan.FromMilliseconds(3000),
    }
  )
  {
    Timeout = System.TimeSpan.FromMilliseconds(3500),
  };
  static private System.Net.Http.HttpClient? client;

  static internal bool IsHttp(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "http" or "https",
  };

  static internal bool IsFile(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "file",
    IsFile: true,
    LocalPath: not "",
  };

  static internal bool IsHttpOrFile(System.Uri uri) => IsHttp(uri) || IsFile(uri);

  static internal System.Uri? ToAbsoluteHttpUri(string uri) => ToAbsoluteUri(uri);
  static internal System.Uri? ToAbsoluteHttpUri(System.Uri uri) => ToAbsoluteUri(uri);

  static internal System.Uri? ToAbsoluteFileUri(string uri) => ToAbsoluteUri(uri, Scheme.File);
  static internal System.Uri? ToAbsoluteFileUri(System.Uri uri) => ToAbsoluteUri(uri, Scheme.File);

  static internal System.Uri? ToAbsoluteHttpOrFileUri(string uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);
  static internal System.Uri? ToAbsoluteHttpOrFileUri(System.Uri uri) => ToAbsoluteUri(uri, Scheme.HttpOrFile);

  static internal System.Uri? ToAbsoluteUri(string uri) => uri is not ""
    && System.Uri.TryCreate(
        uri,
        System.UriKind.Absolute,
        out var url
      )
    ? ToAbsoluteUri(url)
    : default;
  static internal System.Uri? ToAbsoluteUri(System.Uri uri) => IsHttp(uri)
    ? uri
    : default;
  static internal System.Uri? ToAbsoluteUri(
    string uri,
    Scheme scheme
  ) => uri is not ""
    && System.Uri.TryCreate(
      uri,
      System.UriKind.Absolute,
      out var url
    )
    ? ToAbsoluteUri(url, scheme)
    : default;
  static internal System.Uri? ToAbsoluteUri(
    System.Uri uri,
    Scheme scheme
  ) => (scheme & Scheme.Http) is not 0
    && IsHttp(uri)
    || (scheme & Scheme.File) is not 0
    && IsFile(uri)
    ? uri
    : default;

  static internal bool TestHttp(System.Uri uri)
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

  static internal bool TestFile(System.Uri uri) => IsFile(uri)
    && System.IO.Path.Exists(uri.LocalPath);

  static internal void Open() => Open(string.Empty);
  static internal void Open(System.Uri uri)
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
  static internal void Open(string target)
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
