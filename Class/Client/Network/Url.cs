namespace Module.Client.Network;

internal static class Url
{
  internal static bool HostExists(
    string host
  )
  {
    try
    {
      System.Net.Dns.GetHostEntry(
        host,
        System.Net.Sockets.AddressFamily.InterNetwork
      );

      return true;
    }
    catch (System.Net.Sockets.SocketException)
    {
      return false;
    }
  }

  internal static bool Test(
    System.Net.Http.HttpClient client,
    System.Uri url
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
      return false;
    }
    catch (System.InvalidOperationException)
    {
      return false;
    }
  }

  internal static void Open(
    System.Uri uri
  )
  {
    string url = uri
      .ToString()
      .Trim();

    if (
      !string.IsNullOrEmpty(
        url
      )
    )
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
    if (!Environment.Known.Variable.Ssh)
    {
      Invocation.ShellExecute(
        Environment.Known.Application.Chrome,
        target
      );
    }
  }
}
