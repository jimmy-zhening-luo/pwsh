namespace Module.Command.Browse.Test;

using Microsoft.PowerShell.Commands;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Url",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
)]
[Alias("tu")]
[OutputType(typeof(Uri))]
public sealed class TestUrl : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    ValueFromRemainingArguments = true,
    HelpMessage = "The URL to test. If the URL has no scheme, it defaults to 'http'."
  )]
  [AllowEmptyCollection]
  public Uri[] Uri
  {
    get => uris;
    set => uris = value;
  }
  private Uri[] uris = [];

  private protected sealed override void ProcessRecordAction()
  {
    foreach (Uri uri in uris)
    {
      Uri url = new(
        uri.IsAbsoluteUri
          ? string.IsNullOrEmpty(
              uri.Host.Trim()
            )
            ? string.Empty
            : uri.AbsoluteUri.Trim()
          : string.IsNullOrEmpty(
              uri.OriginalString.Trim()
            )
            ? string.Empty
            : "http://" + uri.OriginalString.Trim()
      );

      if (
        !string.IsNullOrEmpty(
          url.OriginalString
        )
      )
      {
        int status = 0;

        try
        {
          try
          {
            ResolveDns(url.Host);
          }
          catch (CmdletInvocationException psException)
          {
            throw psException
              .ErrorRecord
              .Exception;
          }
          catch
          {
            throw;
          }

          try
          {
            status = VisitUrlPath(url);
          }
          catch (CmdletInvocationException psException)
          {
            throw psException
              .ErrorRecord
              .Exception;
          }
          catch
          {
            throw;
          }
        }
        catch (HttpResponseException e)
        {
          status = (int)e.Response.StatusCode;
        }
        catch (System.Net.Http.HttpRequestException)
        {
          status = -1;
        }
        catch (System.ComponentModel.Win32Exception)
        {
          status = -2;
        }
        catch
        {
          throw;
        }

        if (status >= 200 && status < 300)
        {
          WriteObject(url);
        }
      }
    }
  }

  private void ResolveDns(
    string host
  )
  {
    using var ps = ConsoleHost.Create(true);

    AddCommand(
      ps,
      "Resolve-DnsName"
    )
      .AddParameter(
        "Name",
        host
      )
      .AddParameter("DnsOnly")
      .AddParameter("NoHostsFile")
      .AddParameter("QuickTimeout");
    _ = ps.Invoke();

    if (ps.HadErrors)
    {
      throw ps
        .Streams
        .Error[0]
        .Exception;
    }
  }

  private int VisitUrlPath(
    Uri url
  )
  {
    using var ps = ConsoleHost.Create(true);

    AddCommand(
      ps,
      "Invoke-WebRequest"
    )
      .AddParameter(
        "Uri",
        url
      )
      .AddParameter(
        "Method",
        WebRequestMethod.Head
      )
      .AddParameter(
        "ConnectionTimeoutSeconds",
        5
      )
      .AddParameter(
        "MaximumRetryCount",
        0
      )
      .AddParameter("DisableKeepAlive")
      .AddParameter("PreserveHttpMethodOnRedirect");

    return ps
      .Invoke<BasicHtmlWebResponseObject>()[0]
      .StatusCode;
  }
}
