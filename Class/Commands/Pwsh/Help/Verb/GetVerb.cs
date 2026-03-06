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
    [System.ComponentModel.DataAnnotations.Display(Name = "Common")]
    [System.ComponentModel.Description("Common verbs: Add, Clear, Get, New, Set, ..")]
    common,

    [System.ComponentModel.DataAnnotations.Display(Name = "Communications")]
    [System.ComponentModel.Description("Communication verbs: Connect, Read, Write, ..")]
    communications,

    [System.ComponentModel.DataAnnotations.Display(Name = "Data")]
    [System.ComponentModel.Description("Data verbs: Edit, Import, Save, Update, ..")]
    data,

    [System.ComponentModel.DataAnnotations.Display(Name = "Diagnostic")]
    [System.ComponentModel.Description("Diagnostic verbs: Measure, Resolve, Test, ..")]
    diagnostic,

    [System.ComponentModel.DataAnnotations.Display(Name = "Lifecycle")]
    [System.ComponentModel.Description("Lifecycle verbs: Build, Invoke, Start, Stop, ..")]
    lifecycle,

    [System.ComponentModel.DataAnnotations.Display(Name = "Other")]
    [System.ComponentModel.Description("Other verbs: Use, ..")]
    other,

    [System.ComponentModel.DataAnnotations.Display(Name = "Security")]
    [System.ComponentModel.Description("Security verbs: Block, Revoke, ..")]
    security,
  }

  [Parameter(
    Position = default,
    HelpMessage = "Get only the specified verbs or verb patterns"
  )]
  [SupportsWildcards]
  [Tab.Completions("*")]
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
    HelpMessage = "Get only the specified verb groups"
  )]
  public VerbGroup[] Group
  {
    get => [.. groups];
    set
    {
      groups.Clear();

      foreach (var group in value)
      {
        _ = groups.Add(group);
      }
    }
  }
  private readonly HashSet<VerbGroup> groups = [];

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

      foreach (var verbObject in InvokePowerShell<VerbInfo>())
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
