namespace PowerModule.Commands.Pwsh.Help;

using CommonParameter = System.Management.Automation.Internal.CommonParameters;

[Cmdlet(
  VerbsCommon.Get,
  "HelpOnline",
  HelpUri = $"{HelpLink}2096483"
)]
[Alias("m")]
[OutputType(typeof(object))]
sealed public class GetHelpOnline : CoreCommand
{
  const string GetHelpCommand = "Get-Help";

  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Name
  {
    init => topic = string.Join(
      '_',
      value
    );
  }
  string? topic;

  [Parameter]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Parameter
  { private get; init; } = [];

  static List<System.Uri> TryExtractHelpLink(Collection<PSObject> helpContent)
  {
    List<System.Uri> helpLinks = [];

    if (helpContent is [_])
    {
      dynamic pscustomobject = helpContent[default];

      if (
        pscustomobject
          ?.relatedLinks
          ?.navigationLink
        is object[] links
      )
      {
        foreach (var link in links)
        {
          dynamic navigationLink = link;

          if (
            navigationLink
              ?.Uri
              ?.ToString()
            is string uri
            && Client.Network.Url.ToAbsoluteHttpOrFileUri(uri)
            is { } url
          )
          {
            helpLinks.Add(url);
          }
        }
      }
    }

    return helpLinks;
  }

  sealed override private protected void Postprocess()
  {
    if (topic is null)
    {
      WriteObject(
        AddCommand(GetHelpCommand)
          .AddParameter(
            nameof(Name),
            GetHelpCommand
          )
          .Invoke(),
        true
      );

      return;
    }

    var helpContent = AddCommand(GetHelpCommand)
      .AddParameter(
        nameof(Name),
        topic
      )
      .AddParameter(
        nameof(CommonParameter.ErrorAction),
        ActionPreference.SilentlyContinue
      )
      .AddParameter(
        nameof(CommonParameter.ProgressAction),
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
      helpLinks.AddRange(
        TryExtractHelpLink(
          helpContent
        )
      );

      if (Parameter is not [])
      {
        var parameterHelpContent = AddParameter(
          nameof(Parameter),
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

        _ = AddCommand(GetHelpCommand)
          .AddParameter(
            nameof(Name),
            topic
          )
          .AddParameter(
            nameof(CommonParameter.ErrorAction),
            ActionPreference.SilentlyContinue
          )
          .AddParameter(
            nameof(CommonParameter.ProgressAction),
            ActionPreference.SilentlyContinue
          );

        BeginSteppablePipeline();
        ProcessSteppablePipeline();
        EndSteppablePipeline();
      }
    }
  }
}
