namespace Module.Command.Help;

[Cmdlet(
  VerbsCommon.Get,
  "CommandAlias",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096702"
)]
[Alias("galc")]
[OutputType(typeof(AliasInfo))]
public class GetCommandAlias : CoreCommand
{
  [Parameter(
    Position = 0,
    HelpMessage = "Gets the aliases for the specified item. Enter the name of a cmdlet, function, script, file, or executable file. This parameter is called Definition, because it searches for the item name in the Definition property of the alias object."
  )]
  [Alias("Command")]
  [SupportsWildcards]
  [Completions("*")]
  public string[] Definition
  {
    get => definitions;
    set => definitions = value;
  }
  private string[] definitions = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Specifies the scope for which this cmdlet gets aliases. The acceptable values for this parameter are: Global, Local, Script, and a positive integer relative to the current scope (0 through the number of scopes, where 0 is the current scope and 1 is its parent). Global is the default, which differs from Get-Alias where Local is the default."
  )]
  [SupportsWildcards]
  [Completions(
    "global,local,script,0,1,2,3"
  )]
  public string Scope
  {
    get => scope;
    set => scope = value;
  }
  private string scope = "Global";

  [Parameter(
    Position = 2,
    HelpMessage = "Omits the specified items. The value of this parameter qualifies the Definition parameter. Enter a name, a definition, or a pattern, such as 's*'. Wildcards are permitted."
  )]
  [SupportsWildcards]
  public string[] Exclude
  {
    get => exclusions;
    set => exclusions = value;
  }
  private string[] exclusions = [];

  private protected sealed override void TransformParameters()
  {
    HashSet<string> uniqueWildcardTerms = [];
    HashSet<string> uniqueExclusions = [];

    foreach (var definition in definitions)
    {
      if (!string.IsNullOrEmpty(definition))
      {
        uniqueWildcardTerms.Add(
          definition.Contains(
            '*'
          )
            ? definition
            : definition.Length > 2
              ? "*"
                + definition
                + "*"
              : definition
                + "*"
        );
      }
    }

    foreach (var exclusion in exclusions)
    {
      uniqueExclusions.Add(
        exclusion
      );
    }

    definitions = [.. uniqueWildcardTerms];
    exclusions = [.. uniqueExclusions];
  }

  private protected sealed override void AfterEndProcessing()
  {
    SortedDictionary<string, AliasInfo> commandAliasDictionary = [];

    AddCommand(
      "Get-Alias"
    )
      .AddParameter(
        "Definition",
        definitions
      )
      .AddParameter(
        "Scope",
        scope
      );

    if (exclusions.Length != 0)
    {
      PS.AddParameter(
        "Exclude",
        exclusions
      );
    }

    var aliasInfoObjects = PS.Invoke<AliasInfo>();

    if (aliasInfoObjects != null)
    {
      foreach (var aliasInfo in aliasInfoObjects)
      {
        commandAliasDictionary.Add(
          aliasInfo.Definition
            + ":"
            + aliasInfo.Name,
          aliasInfo
        );
      }
    }

    if (commandAliasDictionary.Count != 0)
    {
      WriteObject(
        commandAliasDictionary.Values,
        true
      );
    }
  }
}
