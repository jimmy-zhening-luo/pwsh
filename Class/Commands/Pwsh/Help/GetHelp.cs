namespace PowerModule.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = $"{HelpLink}2096483"
)]
[Alias("m", "man")]
[OutputType(typeof(object))]
sealed public class GetHelpOnline : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  public string[] Name
  {
    init => topic = string.Join(
      '_',
      value
    );
  }
  string topic = string.Empty;

  [Parameter]
  [ValidateNotNullOrWhiteSpace]
  public string[] Parameter
  { private get; init; } = [];

  static IEnumerable<System.Uri> TryExtractHelpLink(Collection<PSObject> helpContent)
  {
    if (helpContent is [])
    {
      yield break;
    }

    dynamic pscustomobject = helpContent[default];

    if (
      pscustomobject
        ?.relatedLinks
        ?.navigationLink is IEnumerable<object> links
    )
    {
      foreach (var link in links)
      {
        dynamic navigationLink = link;

        if (
          navigationLink?.Uri?.ToString() is string uri
          && Client.Network.Url.ToAbsoluteHttpOrFileUri(uri) is { } url
        )
        {
          yield return url;
        }
      }
    }

    yield break;
  }

  sealed override private protected void Postprocess()
  {
    const string GET_HELP = "Get-Help";

    if (topic is "")
    {
      WriteObject(
        AddCommand(GET_HELP)
          .AddParameter(
            StandardParameter.Name,
            GET_HELP
          )
          .Invoke(),
        true
      );

      return;
    }

    var helpContent = AddCommand(GET_HELP)
      .AddParameter(
        StandardParameter.Name,
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

    if (helpContent is not [not null])
    {
      helpContent = default;
    }

    List<System.Uri> helpLinks = [];

    if (helpContent is not null)
    {
      foreach (var link in TryExtractHelpLink(helpContent))
      {
        helpLinks.Add(link);
      }

      if (Parameter is not [])
      {
        var parameterHelpContent = AddParameter(
          "Parameter",
          Parameter
        )
          .Invoke();

        if (parameterHelpContent is [not null])
        {
          helpContent = parameterHelpContent;
        }
      }

      WriteObject(
        helpContent,
        true
      );
    }

    foreach (var helpLink in helpLinks)
    {
      WriteInformation(helpLink.ToString());
    }

    if (!Client.Environment.Variable.InSsh)
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
        ClearCommands();

        _ = AddCommand(GET_HELP)
          .AddParameter(
            StandardParameter.Name,
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

        BeginSteppablePipeline();
        ProcessSteppablePipeline();
        EndSteppablePipeline();
      }
    }
  }
}
