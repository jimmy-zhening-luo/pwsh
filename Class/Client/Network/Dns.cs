namespace Module.Client.Network;

internal static class Dns
{
  internal static bool Resolve(System.Uri uri) => Resolve(uri.Host);
  internal static bool Resolve(string host)
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
}
