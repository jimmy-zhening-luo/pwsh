using System;
using System.Diagnostics;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using System.Net.Http;

namespace Browse
{
  namespace Commands
  {
    [Cmdlet(
      VerbsDiagnostic.Test,
      "UrlWip",
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
    )]
    // [Alias("tu")]
    [OutputType(typeof(Uri))]
    public class TestUrl : PSCmdlet
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
        get => uri;
        set => uri = value;
      }
      private Uri[] uri = [];

      protected override void ProcessRecord()
      {
        foreach (Uri u in uri)
        {
          string us = u.IsAbsoluteUri
            ? u.Host.Trim() == string.Empty
              ? string.Empty
              : u.AbsoluteUri.Trim()
            : u.OriginalString.Trim() == string.Empty
              ? string.Empty
              : "http://" + u.OriginalString.Trim();

          if (us != string.Empty)
          {
            Uri uu = new(us);
            int status = 0;

            try
            {
              using var ps = PowerShell.Create(
                RunspaceMode.CurrentRunspace
              );
              ps
                .AddCommand("Invoke-WebRequest")
                .AddParameter(
                  "Method",
                  WebRequestMethod.Head
                )
                .AddParameter(
                  "PreserveHttpMethodOnRedirect",
                  true
                )
                .AddParameter(
                  "DisableKeepAlive",
                  true
                )
                .AddParameter(
                  "ConnectionTimeoutSeconds",
                  5
                )
                .AddParameter(
                  "MaximumRetryCount",
                  0
                )
                .AddParameter(
                  "ErrorAction",
                  ActionPreference.Stop
                )
                .AddParameter(
                  "Uri",
                  uu
                );

              status = ps.Invoke<BasicHtmlWebResponseObject>()[0].BaseObject.StatusCode;
            }
            catch (HttpResponseException e)
            {
              status = (int)e.Response.StatusCode;
            }
            catch (HttpRequestException)
            {
              status = -1;
            }
            catch
            {
              throw;
            }

            if (status >= 200 && status < 300)
            {
              WriteObject(
                uu,
                true
              );
            }
          }
        }
      }
    }
  }
} // namespace Browse
