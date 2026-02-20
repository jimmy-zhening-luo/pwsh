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
      topic
    );

    WriteObject(
      helpContent,
      true
    );
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
    string topic
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

    return ps.Invoke();
  }

  private System.Uri? TryHelpLink(
    Collection<PSObject> helpContent
  )
  {
    try
    {
      return new System.Uri(
        helpContent[0].relatedLinks.navigationLink.Uri
      );
    }
    catch
    {
      return null;
    }
  }
}
