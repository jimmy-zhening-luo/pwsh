namespace Module.Commands.Pwsh.Help.Alias;

[Cmdlet(
  VerbsCommon.Get,
  "CommandAlias",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096702"
)]
[Alias("galc")]
[OutputType(typeof(AliasInfo))]
sealed public class GetCommandAlias : CoreCommand
{
  [Parameter(
    Position = default
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.Completions("*")]
  public string[] Definition
  {
    private get => [.. definitions];
    set
    {
      definitions.Clear();

      foreach (var definition in value)
      {
        _ = definitions.Add(
          definition.Contains('*')
            ? definition
            : definition.Length > 2
              ? $"*{definition}*"
              : $"{definition}*"
        );
      }
    }
  }
  private readonly HashSet<string> definitions = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Specifies the scope for which this cmdlet gets aliases. The acceptable values for this parameter are: Global, Local, Script, and a positive integer relative to the current scope (0 through the number of scopes, where 0 is the current scope and 1 is its parent). Global is the default, which differs from Get-Alias where Local is the default."
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.Completions(
    "global",
    "local",
    "script",
    "0",
    "1",
    "2",
    "3"
  )]
  public string Scope
  {
    private get;
    set;
  } = "Global";

  [Parameter(
    Position = 2
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  public string[] Exclude
  {
    private get => [.. exclusions];
    set
    {
      exclusions.Clear();

      foreach (var exclusion in value)
      {
        if (exclusion is not "")
        {
          _ = exclusions.Add(exclusion);
        }
      }
    }
  }
  private readonly HashSet<string> exclusions = [];

  sealed private protected override void Postprocess()
  {
    if (Definition is [])
    {
      _ = definitions.Add("*");
    }

    SortedDictionary<string, AliasInfo> commandAliasDictionary = [];

    _ = AddCommand(
      @"Microsoft.PowerShell.Utility\Get-Alias"
    )
      .AddParameter(
        "Definition",
        Definition
      )
      .AddParameter(
        "Scope",
        Scope
      );

    if (Exclude is not [])
    {
      _ = AddParameter("Exclude", Exclude);
    }

    foreach (var aliasInfo in InvokePowerShell<AliasInfo>())
    {
      var key = string.Concat(
        aliasInfo.Definition,
        ':',
        aliasInfo.Name
      );

      if (
        !commandAliasDictionary.ContainsKey(
          key
        )
      )
      {
        commandAliasDictionary.Add(
          key,
          aliasInfo
        );
      }
    }

    if (commandAliasDictionary.Count is not 0)
    {
      WriteObject(
        commandAliasDictionary.Values,
        true
      );
    }
  }
}
