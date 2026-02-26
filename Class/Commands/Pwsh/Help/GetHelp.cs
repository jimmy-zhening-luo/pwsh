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
  private static readonly string AboutBaseUrl = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about";

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

  private static System.Uri? ResolveAboutArticle(
    string topic,
    System.Net.Http.HttpClient client
  )
  {
    System.Uri testUri = new(
      $"{AboutBaseUrl}/{topic}"
    );

    return Client.Network.Url.Test(
      testUri,
      client
    )
      ? testUri
      : default;
  }

  private static List<System.Uri>? TryHelpLink(
    Collection<PSObject> helpContent
  )
  {
    if (helpContent is null or [])
    {
      return default;
    }

    dynamic pscustomobject = helpContent[0];

    if (
      pscustomobject
        ?.relatedLinks
        ?.navigationLink is not null
        and IEnumerable<object> links
    )
    {
      List<System.Uri> urls = [];

      foreach (dynamic link in links)
      {
        if (
          link?.Uri is not null
          and object noteProperty
        )
        {
          var uriString = noteProperty.ToString();

          if (
            uriString is not null
            and not ""
            && (
              uriString.StartsWith(
                "https://"
              )
              || uriString.StartsWith(
                "http://"
              )
            )
          )
          {
            System.Uri url = new(
              uriString
            );

            if (
              url is
              {
                IsAbsoluteUri: true,
                Host: not ""
              }
            )
            {
              urls.Add(
                url
              );
            }
          }
        }
      }

      return urls is []
        ? default
        : urls;
    }
    else
    {
      return default;
    }
  }

  private protected sealed override void Postprocess()
  {
    if (topic is not "")
    {
      var helpContent = GetHelpContent(
        topic,
        []
      );

      if (helpContent is not [_])
      {
        helpContent = default;
      }

      List<System.Uri> helpLinks = [];

      if (helpContent is not null)
      {
        var helpContentLinks = TryHelpLink(
          helpContent
        );

        if (helpContentLinks is not null)
        {
          foreach (var helpLink in helpContentLinks)
          {
            helpLinks.Add(
              helpLink
            );
          }
        }

        if (Parameter is not [])
        {
          var parameterHelpContent = GetHelpContent(
            topic,
            Parameter
          );

          if (parameterHelpContent is not null)
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
          WriteLog(
            helpLink.ToString()
          );
        }
      }

      if (!Client.Environment.Known.Variable.InSsh)
      {
        if (helpLinks is not [])
        {
          foreach (var helpLink in helpLinks)
          {
            Client.Network.Url.Open(
              helpLink
            );
          }
        }
        else if (helpContent is not null)
        {
          _ = GetHelpContent(
            topic,
            [],
            true
          );
        }
      }
    }
    else
    {
      WriteObject(
        AddCommand(
          "Get-Help"
        )
          .AddParameter(
            "Name",
            "Get-Help"
          )
          .Invoke(),
        true
      );
    }
  }

  private Collection<PSObject> GetHelpContent(
    string topic,
    string[] parameters,
    bool online = default
  )
  {
    using var ps = PowerShellHost.Create(
      true
    );

    _ = AddCommand(
      ps,
      "Get-Help"
    )
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

    if (parameters is not [])
    {
      _ = ps.AddParameter(
        "Parameter",
        parameters
      );
    }

    if (
      online
      && parameters is []
    )
    {
      try
      {
        _ = ps.AddParameter(
          "Online"
        );

        return [];
      }
      catch
      {
        return [];
      }
    }
    else
    {
      return ps.Invoke();
    }
  }
}
