namespace Module.Commands.Pwsh.Help.Verb;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097026"
)]
[OutputType(typeof(VerbInfo))]
[OutputType(typeof(string))]
public sealed partial class GetVerb : CoreCommand
{
  [Parameter(
    Position = 0,
    HelpMessage = "Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed."
  )]
  [SupportsWildcards]
  [Completions(
    ["*"]
  )]
  public string[] Verb { get; set; } = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Gets only the specified groups. Enter the name of a group. Wildcards aren't allowed."
  )]
  [EnumCompletions(
    typeof(VerbGroup)
  )]
  public string[] Group { get; set; } = [];

  private protected sealed override void AfterEndProcessing()
  {
    HashSet<string> uniqueWildcardTerms = [];
    HashSet<string> uniqueGroups = [];

    foreach (var verb in Verb)
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

    foreach (var group in Group)
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

    Verb = [.. uniqueWildcardTerms];
    Group = [.. uniqueGroups];

    if (
      Verb.Length == 0
      || Verb.Length == 1
      && Verb[0] == "*"
    )
    {
      AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          "*"
        );

      if (Group.Length != 0)
      {
        PS.AddParameter(
          "Group",
          Group
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
    else
    {
      SortedDictionary<string, VerbInfo> verbDictionary = [];

      AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          Verb
        );

      if (Group.Length != 0)
      {
        PS.AddParameter(
          "Group",
          Group
        );
      }

      var verbObjects = PS.Invoke<VerbInfo>();

      if (verbObjects != null)
      {
        foreach (var verbObject in verbObjects)
        {
          if (
            !verbDictionary.ContainsKey(
              verbObject.Verb
            )
          )
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
  }
}
