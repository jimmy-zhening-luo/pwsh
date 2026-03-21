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
  const string DefaultScope = "global";

  [Parameter(Position = default)]
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
            Client.StringInput.Wildcard,
            System.StringComparison.Ordinal
          )
            ? definition
            : definition.Length > 2
              ? Client.StringInput.StringWildcard+ definition
                + Client.StringInput.StringWildcard
              : definition
                + Client.StringInput.StringWildcard
        );
      }
    }
  }
  readonly HashSet<string> definitions = [];

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  [ArgumentCompletions(
    DefaultScope,
    "local",
    "script",
    "0",
    "1",
    "2",
    "3"
  )]
  public string Scope
  { private get; init; } = DefaultScope;

  [Parameter(Position = 2)]
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
        Client.StringInput.StringWildcard
      );
    }

    SortedDictionary<string, AliasInfo> commandAliasDictionary = new(
      System.StringComparer.OrdinalIgnoreCase
    );

    _ = AddCommand(
      $@"{StandardModule.Utility}\Get-Alias"
    )
      .AddParameter(
        nameof(Definition),
        Definition
      )
      .AddParameter(
        nameof(Scope),
        Scope
      );

    if (Exclude is not [])
    {
      _ = AddParameter(
        nameof(Exclude),
        Exclude
      );
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
