using System;
using System.Diagnostics;
using System.Management.Automation;

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
              : u.AbsoluteUri
            : u.OriginalString.Trim() == string.Empty
              ? string.Empty
              : 'http://u.OriginalString';

          
        }
      }
    }
  }
} // namespace Browse
