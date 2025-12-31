using System;
using System.ComponentModel;
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
      "Url",
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097126"
    )]
    [Alias("tu")]
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
              try
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
                    uu.Host
                  )
                  .AddParameter(
                    "Server",
                    "1.1.1.1"
                  )
                  .AddParameter("DnsOnly")
                  .AddParameter("NoHostsFile")
                  .AddParameter("QuickTimeout");
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
                    uu
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

                status = ps.Invoke<BasicHtmlWebResponseObject>()[0].StatusCode;
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
              WriteWarning("hi");
              status = -2;
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
