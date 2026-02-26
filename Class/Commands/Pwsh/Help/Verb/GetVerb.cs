namespace Module.Commands.Pwsh.Help.Verb;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097026"
)]
[Alias("vb")]
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
  public string[] Verb
  {
    get => [.. verbs];
    set
    {
      foreach (var verb in value)
      {
        if (verb is not "")
        {
          _ = verbs.Add(
            verb.Contains(
              '*'
            )
              ? verb
              : verb.Length > 2
                ? $"*{verb}*"
                : $"{verb}*"
          );
        }
      }
    }
  }
  private readonly HashSet<string> verbs = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Gets only the specified groups. Enter the name of a group. Wildcards aren't allowed."
  )]
  [EnumCompletions(
    typeof(VerbGroup)
  )]
  public string[] Group
  {
    get => [.. groups];
    set
    {
      foreach (var group in value)
      {
        if (
          System.Enum.TryParse<VerbGroup>(
            group,
            true,
            out var parsedGroup
          )
        )
        {
          _ = groups.Add(
            parsedGroup.ToString()
          );
        }
      }
    }
  }
  private readonly HashSet<string> groups = [];

  private protected sealed override void Postprocess()
  {
    if (Verb is [] or ["*"])
    {
      _ = AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          "*"
        );

      if (Group is not [])
      {
        _ = PS.AddParameter(
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

      _ = AddCommand(
        "Get-Verb"
      )
        .AddParameter(
          "Verb",
          Verb
        );

      if (Group is not [])
      {
        _ = PS.AddParameter(
          "Group",
          Group
        );
      }

      var verbObjects = PS.Invoke<VerbInfo>();

      if (verbObjects is not null)
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

      if (verbDictionary.Count is not 0)
      {
        WriteObject(
          verbDictionary.Values,
          true
        );
      }
    }
  }
}
