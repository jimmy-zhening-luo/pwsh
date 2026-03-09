namespace PowerModule.Client.Network;

static internal class Dns
{
  static internal bool Resolve(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: "http" or "https",
    Host: var host,
  }
    && Resolve(host);
  static internal bool Resolve(string host)
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
