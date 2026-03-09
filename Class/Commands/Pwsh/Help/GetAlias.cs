namespace PowerModule.Commands.Pwsh.Help;

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
  public Collection<string> Definition
  {
    private get => [.. definitions];
    init
    {
      definitions.Clear();

      if (value is null)
      {
        return;
      }

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
  { private get; init; } = "Global";

  [Parameter(
    Position = 2
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  public Collection<string> Exclude
  { private get; init; } = [];

  sealed override private protected void Postprocess()
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
