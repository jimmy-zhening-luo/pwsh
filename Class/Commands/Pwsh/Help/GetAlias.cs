namespace PowerModule.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "CommandAlias",
  HelpUri = $"{HelpLink}2096702"
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
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Definition
  {
    private get => [.. definitions];
    init
    {
      definitions.Clear();

      foreach (var definition in value)
      {
        _ = definitions.Add(
          definition.Contains(
            '*',
            System.StringComparison.Ordinal
          )
            ? definition
            : definition.Length > 2
              ? $"*{definition}*"
              : $"{definition}*"
        );
      }
    }
  }
  readonly HashSet<string> definitions = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Specifies the scope for which this cmdlet gets aliases. The acceptable values for this parameter are: Global, Local, Script, and a positive integer relative to the current scope (0 through the number of scopes, where 0 is the current scope and 1 is its parent). Global is the default, which differs from Get-Alias where Local is the default."
  )]
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
  { private get; init; } = "global";

  [Parameter(
    Position = 2
  )]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Exclude
  { private get; init; } = [];

  [System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Style",
    "IDE0028: Use collection initializers or expressions",
    Justification = "Incorrect suggestion, .NET fix expected in 10.0.3xx: https://github.com/dotnet/Roslyn/issues/82586"
  )]
  sealed override private protected void Postprocess()
  {
    if (Definition is [])
    {
      _ = definitions.Add(
        Client.StringInput.Wildcard
      );
    }

    SortedDictionary<string, AliasInfo> commandAliasDictionary = new(
      System.StringComparer.OrdinalIgnoreCase
    );

    _ = AddCommand(
      $@"{StandardModule.Utility}\Get-Alias"
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

      _ = commandAliasDictionary.TryAdd(
        key,
        aliasInfo
      );
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
