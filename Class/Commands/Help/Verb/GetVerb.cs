namespace Module.Commands.Help.Verb;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097026"
)]
[OutputType(typeof(VerbInfo))]
[OutputType(typeof(string))]
public sealed partial class GetVerb : CoreCommand
{
  private bool performSearch;

  [Parameter(
    Position = 0,
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
    HashSet<string> uniqueWildcardTerms = [];
    HashSet<string> uniqueGroups = [];

    foreach (var verb in verbs)
    {
      if (!string.IsNullOrEmpty(verb))
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
    }

    foreach (var group in groups)
    {
      if (
        System.Enum.TryParse<VerbGroup>(
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
          if (!verbDictionary.ContainsKey(verbObject.Verb))
          {
            verbDictionary.Add(
              verbObject.Verb,
              verbObject
            );
          }
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
