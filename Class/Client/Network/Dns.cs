namespace Module.Client.Network;

internal static class Dns
{
  internal static bool Resolve(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "http" or "https",
    Host: var host,
  }
    && Resolve(host);
  internal static bool Resolve(string host)
  {
    if (host is "")
    {
      return default;
    }

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
