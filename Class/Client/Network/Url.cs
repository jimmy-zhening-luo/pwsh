namespace PowerModule.Client.Network;

static class Url
{
  static System.Net.Http.HttpClient HttpClient => client ??= new System.Net.Http.HttpClient(
    HttpHandler
  )
  {
    Timeout = System.TimeSpan.FromMilliseconds(3500),
  };
  static System.Net.Http.HttpClient? client;

  static internal System.Net.Http.SocketsHttpHandler HttpHandler => handler ??= new System.Net.Http.SocketsHttpHandler()
  {
    PooledConnectionLifetime = System.TimeSpan.FromMinutes(2),
    ConnectTimeout = System.TimeSpan.FromMilliseconds(3000),
  };
  static System.Net.Http.SocketsHttpHandler? handler;

  static internal bool IsHttp(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: Scheme.Http or Scheme.Https,
  };

  static internal bool IsFile(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: Scheme.File,
    IsFile: true,
    LocalPath: not "",
  };

  static internal bool IsHttpOrFile(System.Uri uri) => IsHttp(uri) || IsFile(uri);

  static internal System.Uri? ToAbsoluteHttpUri(string input) => TryParse(input) is { } uri
  && IsHttp(uri)
    ? uri
    : default;

  static internal System.Uri? ToAbsoluteFileUri(string input) => TryParse(input) is { } uri
  && IsFile(uri)
    ? uri
    : default;

  static internal System.Uri? ToAbsoluteHttpOrFileUri(string input) => TryParse(input) is { } uri
  && IsHttpOrFile(uri)
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
    if (!Environment.Variable.InSsh)
    {
      Start.ShellExecute(
        Environment.Application.Chrome,
        target
      );
    }
  }

  static System.Uri? TryParse(string uriString) => System.Uri.TryCreate(
    uriString,
    System.UriKind.Absolute,
    out var uri
  )
    ? uri
    : default;
}
