namespace PowerModule.Client.Network;

static class Dns
{
  static internal bool Resolve(System.Uri uri) => uri is
  {
    IsAbsoluteUri: true,
    Scheme: Scheme.Http or Scheme.Https,
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
