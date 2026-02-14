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
}
