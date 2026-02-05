namespace Module.Command.Help;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097026"
)]
[OutputType(typeof(VerbInfo))]
[OutputType(typeof(string))]
public class GetVerb : CoreCommand
{
  public enum VerbGroup
  {
    Common,
    Communications,
    Data,
    Diagnostic,
    Lifecycle,
    Other,
    Security,
    Service,
    Settings,
    Support,
    System,
    Utility
  }

  private bool performSearch;

  [Parameter(
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed."
  )]
  [SupportsWildcards]
  [Completions("*")]
  public string[] Verb
  {
    get => verbs;
    set => verbs = value;
  }
  private string[] verbs = [];

  [Parameter(
    Position = 1,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "Gets only the specified groups. Enter the name of a group. Wildcards aren't allowed."
  )]
  [EnumCompletions(
    typeof(VerbGroup)
  )]
  public string[] Group
  {
    get => groups;
    set => groups = value;
  }
  private string[] groups = [];

  private protected sealed override void TransformParameters()
  {
    HashSet<string> uniqueTerms = [];
    HashSet<string> uniqueWildcardTerms = [];
    HashSet<string> uniqueGroups = [];

    foreach (var verb in verbs)
    {
      if (!string.IsNullOrEmpty(verb))
      {
        uniqueTerms.Add(verb);
      }
    }

    foreach (var verb in uniqueTerms)
    {
      uniqueWildcardTerms.Add(
        verb.Contains(
          '*'
        )
          ? verb
          : verb.Length > 2
            ? "*"
              + verb
              + "*"
            : verb
              + "*"
      );
    }

    foreach (var group in groups)
    {
      if (
        Enum.TryParse<VerbGroup>(
          group,
          true,
          out var parsedGroup
        )
      )
      {
        uniqueGroups.Add(
          parsedGroup.ToString()
        );
      }
    }

    verbs = [.. uniqueWildcardTerms];
    groups = [.. uniqueGroups];

    if (
      verbs.Length > 1
      || verbs.Length == 1
      && verbs[0] != "*"
    )
    {
      performSearch = true;
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    if (performSearch)
    {
      SortedDictionary<string, VerbInfo> verbDictionary = [];

      AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          verbs
        );

      if (groups.Length != 0)
      {
        PS.AddParameter(
          "Group",
          groups
        );
      }

      var verbObjects = PS.Invoke<VerbInfo>();

      if (verbObjects != null)
      {
        foreach (var verbObject in verbObjects)
        {
          verbDictionary.Add(
            verbObject.Verb,
            verbObject
          );
        }
      }

      if (verbDictionary.Count != 0)
      {
        WriteObject(
          verbDictionary.Values,
          true
        );
      }
    }
    else
    {
      AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          "*"
        );

      if (groups.Length != 0)
      {
        PS.AddParameter(
          "Group",
          groups
        );
      }

      WriteObject(
        AddCommand(
          "Select-Object"
        )
          .AddParameter(
            "ExpandProperty",
            "Verb"
          )
          .AddCommand(
            "Sort-Object"
          )
          .Invoke(),
        true
      );
    }
  }
}
