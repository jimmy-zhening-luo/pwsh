namespace Module.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096483"
)]
[Alias("m", "man")]
[OutputType(typeof(object))]
public sealed class GetHelpOnline : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  public string[] Name
  {
    get => [topic];
    set => topic = string.Join(
      '_',
      value
    );
  }
  private string topic = string.Empty;

  [Parameter]
  public string[] Parameter { get; set; } = [];

  private static List<System.Uri>? TryExtractHelpLink(Collection<PSObject> helpContent)
  {
    if (helpContent is null or [])
    {
      return default;
    }

    dynamic pscustomobject = helpContent[0];

    List<System.Uri> urls = [];

    if (
      pscustomobject
        ?.relatedLinks
        ?.navigationLink is IEnumerable<object> links
    )
    {
      foreach (dynamic link in links)
      {
        if (
          link?.Uri is not null
          and object noteProperty
        )
        {
          var uriString = noteProperty.ToString();

          if (
            uriString is not (null or "")
            && (
              uriString.StartsWith("https://")
              || uriString.StartsWith("http://")
            )
          )
          {
            System.Uri url = new(uriString);

            if (
              url is
              {
                IsAbsoluteUri: true,
                Host: not ""
              }
            )
            {
              urls.Add(url);
            }
          }
        }
      }
    }

    return urls is [] ? default : urls;
  }

  private protected sealed override void Postprocess()
  {
    const string GET_HELP = "Get-Help";

    if (topic is "")
    {
      WriteObject(
        AddCommand(GET_HELP)
          .AddParameter(
            "Name",
            GET_HELP
          )
          .Invoke(),
        true
      );

      return;
    }

    var helpContent = AddCommand(GET_HELP)
      .AddParameter(
        "Name",
        topic
      )
      .AddParameter(
        "ErrorAction",
        ActionPreference.SilentlyContinue
      )
      .AddParameter(
        "ProgressAction",
        ActionPreference.SilentlyContinue
      )
      .Invoke();

    if (helpContent is not [_])
    {
      helpContent = default;
    }

    List<System.Uri> helpLinks = [];

    if (helpContent is not null)
    {
      var helpContentLinks = TryExtractHelpLink(helpContent);

      if (helpContentLinks is not null)
      {
        foreach (var link in helpContentLinks)
        {
          helpLinks.Add(link);
        }
      }

      if (Parameter is not [])
      {
        var parameterHelpContent = AddParameter(
          "Parameter",
          Parameter
        )
          .Invoke();

        if (parameterHelpContent is [_])
        {
          helpContent = parameterHelpContent;
        }
      }

      WriteObject(
        helpContent,
        true
      );
    }

    if (helpLinks is not [])
    {
      foreach (var helpLink in helpLinks)
      {
        WriteLog(helpLink.ToString());
      }
    }

    if (!Client.Environment.Known.Variable.InSsh)
    {
      if (helpLinks is not [])
      {
        foreach (var helpLink in helpLinks)
        {
          Client.Network.Url.Open(helpLink);
        }
      }
      else if (helpContent is not null)
      {
        Clear();

        AddCommand(GET_HELP)
          .AddParameter(
            "Name",
            topic
          )
          .AddParameter(
            "ErrorAction",
            ActionPreference.SilentlyContinue
          )
          .AddParameter(
            "ProgressAction",
            ActionPreference.SilentlyContinue
          );

        SteppablePipeline.Begin(this);

        _ = steppablePipeline.Process();
      }
    }
  }
}
