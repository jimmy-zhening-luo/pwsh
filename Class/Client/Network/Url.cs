namespace Module.Client.Network;

internal static class Url
{
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

  internal static void Open(System.Uri uri)
  {
    if (
      uri.ToString() is string url
      && !string.IsNullOrWhiteSpace(url)
    )
    {
      Open(url);
    }
  }

  internal static void Open(
    [System.Diagnostics.CodeAnalysis.StringSyntax(
      System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.Uri
    )]
    string target
  )
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
