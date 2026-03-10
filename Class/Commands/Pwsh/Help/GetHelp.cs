namespace PowerModule.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096483"
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
  public Collection<string> Name
  {
    init => topic = string.Join(
      '_',
      value
    );
  }
  string topic = string.Empty;

  [Parameter]
  [ValidateNotNullOrWhiteSpace]
  public Collection<string> Parameter
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
      foreach (dynamic link in links)
      {
        if (
          link?.Uri?.ToString() is string helpLink
          && Client.Network.Url.ToAbsoluteHttpOrFileUri(helpLink) is { } uri
        )
        {
          yield return uri;
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

        BeginSteppablePipeline();
        ProcessSteppablePipeline();
        EndSteppablePipeline();
      }
    }
  }
}
