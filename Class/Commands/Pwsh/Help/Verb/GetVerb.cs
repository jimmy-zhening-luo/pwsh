namespace Module.Commands.Pwsh.Help.Verb;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097026"
)]
[Alias("vb")]
[OutputType(typeof(VerbInfo))]
[OutputType(typeof(string))]
public sealed class GetVerb : CoreCommand
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

  [Parameter(
    Position = default,
    HelpMessage = "Gets only the specified verbs. Enter the name of a verb or a name pattern. Wildcards are allowed."
  )]
  [SupportsWildcards]
  [Completions(["*"])]
  public string[] Verb
  {
    get => [.. verbs];
    set
    {
      verbs.Clear();

      foreach (var verb in value)
      {
        if (verb is not "")
        {
          _ = verbs.Add(
            verb.Contains('*')
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
  [EnumCompletions(typeof(VerbGroup))]
  public string[] Group
  {
    get => [.. groups];
    set
    {
      groups.Clear();

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
          _ = groups.Add(parsedGroup.ToString());
        }
      }
    }
  }
  private readonly HashSet<string> groups = [];

  private protected sealed override void Postprocess()
  {
    const string GET_VERB = @"Microsoft.PowerShell.Utility\Get-Verb";

    if (Verb is [] or ["*"])
    {
      _ = AddCommand(GET_VERB)
        .AddParameter(
          "Verb",
          "*"
        );

      if (Group is not [])
      {
        _ = AddParameter(
          "Group",
          Group
        );
      }

      WriteObject(
        AddCommand(
          @"Microsoft.PowerShell.Utility\Select-Object"
        )
          .AddParameter(
            "ExpandProperty",
            "Verb"
          )
          .AddCommand(
            @"Microsoft.PowerShell.Utility\Sort-Object"
          )
          .Invoke(),
        true
      );
    }
    else
    {
      SortedDictionary<string, VerbInfo> verbDictionary = [];

      _ = AddCommand(GET_VERB)
        .AddParameter(
          "Verb",
          Verb
        );

      if (Group is not [])
      {
        _ = AddParameter(
          "Group",
          Group
        );
      }

      var verbObjects = InvokePowerShell<VerbInfo>();

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
