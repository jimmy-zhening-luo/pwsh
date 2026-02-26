namespace Module.Client.Network;

internal static class Url
{
  internal static bool HostExists(
    System.Uri uri
  ) => HostExists(
    uri.Host
  );
  internal static bool HostExists(
    string host
  )
  {
    try
    {
      _ = System.Net.Dns.GetHostEntry(
        host,
        System.Net.Sockets.AddressFamily.InterNetwork
      );

      return true;
    }
    catch (System.Net.Sockets.SocketException)
    {
      return default;
    }
  }

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

  internal static void Open(
    System.Uri uri
  )
  {
    var url = uri
      .ToString()
      .Trim();

    if (url is not "")
    {
      Open(
        url
      );
    }
  }

  internal static void Open(
    string target
  )
  {
    if (!Environment.Known.Variable.InSsh)
    {
      Invocation.ShellExecute(
        Environment.Known.Application.Chrome,
        target
      );
    }
  }
}
