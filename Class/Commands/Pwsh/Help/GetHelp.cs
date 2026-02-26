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
    Position = 0,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  public string[] Name { get; set; } = [];

  [Parameter]
  public string[] Parameter { get; set; } = [];

  private static System.Uri? ResolveAboutArticle(
    System.Net.Http.HttpClient client,
    string topic
  )
  {
    System.Uri testUri = new(
      $"{AboutBaseUrl}/{topic}"
    );

    return Client.Network.Url.Test(
      client,
      testUri
    )
      ? testUri
      : null;
  }

  private static List<System.Uri>? TryHelpLink(
    Collection<PSObject> helpContent
  )
  {
    if (helpContent is null or [])
    {
      return null;
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
              url.IsAbsoluteUri
              && url.Host is not ""
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
        ? null
        : urls;
    }
    else
    {
      return null;
    }
  }

  private protected sealed override void Postprocess()
  {
    if (
      Name is not []
      and not [""]
    )
    {
      var topic = string.Join(
        '_',
        Name
      );
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

      if (!Client.Environment.Known.Variable.Ssh)
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

    AddCommand(
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
      ps.AddParameter(
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
        ps.AddParameter(
          "Online"
        );

        return ps.Invoke();
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
