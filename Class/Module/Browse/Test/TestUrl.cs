namespace Module.Browse.Test
{
  namespace Commands
  {
    using System;
    using System.ComponentModel;
    using System.Management.Automation;
    using Microsoft.PowerShell.Commands;
    using System.Net.Http;

    [Cmdlet(
      VerbsDiagnostic.Test,
      "Url",
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
    )]
    [Alias("tu")]
    [OutputType(typeof(Uri))]
    public class TestUrl : PSCoreCommand
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

      protected override void ProcessRecord()
      {
        foreach (Uri uri in uris)
        {
          Uri fullUrl = new(
            uri.IsAbsoluteUri
              ? uri.Host.Trim() == string.Empty
                ? string.Empty
                : uri.AbsoluteUri.Trim()
              : uri.OriginalString.Trim() == string.Empty
                ? string.Empty
                : "http://" + uri.OriginalString.Trim()
          );

          if (fullUrl.OriginalString != string.Empty)
          {
            int status = 0;

            try
            {
              try
              {
                ResolveDns(fullUrl.Host);
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
                status = VisitUrlPath(fullUrl);
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
            catch (HttpRequestException)
            {
              status = -1;
            }
            catch (Win32Exception)
            {
              status = -2;
            }
            catch
            {
              throw;
            }

            if (status >= 200 && status < 300)
            {
              WriteObject(fullUrl);
            }
          }
        }
      }

      private void ResolveDns(string host)
      {
        using var ps = PowerShell.Create(
          RunspaceMode.CurrentRunspace
        );
        ps
          .AddCommand(
            SessionState
              .InvokeCommand
              .GetCommand(
                "Resolve-DnsName",
                CommandTypes.Cmdlet
              )
          )
          .AddParameter(
            "Name",
            host
          )
          .AddParameter(
            "Server",
            "1.1.1.1"
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

      private int VisitUrlPath(Uri fullUrl)
      {
        using var ps = PowerShell.Create(
          RunspaceMode.CurrentRunspace
        );
        ps
          .AddCommand(
            SessionState
              .InvokeCommand
              .GetCommand(
                "Invoke-WebRequest",
                CommandTypes.Cmdlet
              )
          )
          .AddParameter(
            "Uri",
            fullUrl
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

        return ps.Invoke<BasicHtmlWebResponseObject>()[0].StatusCode;
      }
    }
  }
}
