namespace Module.Commands.Help;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096483"
)]
[Alias("m", "man")]
[OutputType(typeof(object))]
public sealed class GetHelpOnline : CoreCommand
{
  private static string AboutBaseUrl => "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about";

  [Parameter(
    Position = 0,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [Completions("*")]
  public string[] Name
  {
    get => names;
    set => names = value;
  }
  private string[] names = [];

  [Parameter]
  public string[] Parameter
  {
    get => parameters;
    set => parameters = value;
  }
  private string[] parameters = [];

  private static System.Uri? ResolveAboutArticle(
    System.Net.Http.HttpClient client,
    string topic
  )
  {
    System.Uri testUri = new(AboutBaseUrl + "/" + topic);

    return Client.Network.Url.Test(
      client,
      testUri
    )
      ? testUri
      : null;
  }

  private protected sealed override bool ValidateParameters() => names.Length != 0
    && (
      names.Length != 1
      || !string.IsNullOrEmpty(
        names[0]
      )
    );

  private protected sealed override void AfterEndProcessing()
  {
    var topic = string.Join(
      '_',
      names
    );
    var helpContent = GetHelpContent(
      topic,
      []
    );

    if (
      helpContent is not null
      && helpContent.Count != 1
    )
    {
      helpContent = null;
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
          helpLinks.Add(helpLink);
        }
      }

      if (parameters.Length != 0)
      {
        var parameterHelpContent = GetHelpContent(
          topic,
          parameters
        );
  
        if (parameterHelpContent is not null)
        {
          var helpContent = parameterHelpContent;
        }
      }

      WriteObject(
        helpContent,
        true
      );
    }

    if (helpLinks.Count != 0)
    {
      foreach (var helpLink in helpLinks)
      {
        WriteObject(
          helpLink.ToString()
        );
      }
    }

    if (!Client.Environment.Known.Variable.Ssh)
    {
      if (helpLinks.Count != 0)
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

  private protected sealed override void DefaultAction() => WriteObject(
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

  private Collection<PSObject> GetHelpContent(
    string topic,
    string[] parameters,
    bool online = false
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

    if (parameters.Length != 0)
    {
      ps.AddParameter(
        "Parameter",
        parameters
      );
    }
    else if (online)
    {
      ps.AddParameter(
        "Online"
      );
    }

    return ps.Invoke();
  }

  private List<System.Uri>? TryHelpLink(
    Collection<PSObject> helpContent
  )
  {
    try
    {
      dynamic pscustomobject = helpContent[0];
      dynamic relatedLinks = pscustomobject.relatedLinks;
      PSObject[] navigationLink = relatedLinks.navigationLink;

      List<System.Uri> urls = [];

      foreach (dynamic link in navigationLink)
      {
        dynamic uri = link.Uri;
        var uriString = uri.ToString();

        if (
          !string.IsNullOrEmpty(
            uriString
          )
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
          var url = new System.Uri(
            uriString
          );

          if (
            url.IsAbsoluteUri
            && !string.IsNullOrEmpty(
              url.Host
            )
          )
          {
            urls.Add(
              url
            );
          }
        }
      }

      if (urls.Count != 0)
      {
        return urls;
      }

      return null;
    }
    catch
    {
      return null;
    }
  }
}
