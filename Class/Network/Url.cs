namespace Module.Network;

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
    }
    catch (System.Net.Sockets.SocketException)
    {
      return false;
    }
    catch
    {
      throw;
    }

    return true;
  }

  internal static bool Test(
    System.Net.Http.HttpClient client,
    Uri url
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
    catch
    {
      throw;
    }
  }
}
