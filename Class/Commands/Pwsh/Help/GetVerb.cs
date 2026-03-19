namespace PowerModule.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "VerbList",
  HelpUri = $"{HelpLink}2097026"
)]
[Alias("vb")]
[OutputType(typeof(VerbInfo))]
[OutputType(typeof(string))]
sealed public class GetVerb : CoreCommand
{
  public enum VerbGroup
  {
    common,
    communications,
    data,
    diagnostic,
    lifecycle,
    other,
    security,
  }

  [Parameter(
    Position = default,
    HelpMessage = "Get only the specified verbs or verb patterns"
  )]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Verb
  {
    private get => [.. verbs];
    init
    {
      verbs.Clear();

      foreach (var verb in value)
      {
        _ = verbs.Add(
          verb.Contains(
            '*',
            System.StringComparison.Ordinal
          )
            ? verb
            : verb.Length > 2
              ? $"*{verb}*"
              : $"{verb}*"
        );
      }
    }
  }
  readonly HashSet<string> verbs = [];

  [Parameter(
    Position = 1,
    HelpMessage = "Get only the specified verb groups"
  )]
  [ValidateNotNullOrEmpty]
  public VerbGroup[] Group
  { private get; init; } = [];

  sealed override private protected void Postprocess()
  {
    const string GET_VERB = @"Microsoft.PowerShell.Utility\Get-Verb";

    if (
      Verb is []
      or [Client.StringInput.Wildcard]
    )
    {
      _ = AddCommand(GET_VERB)
        .AddParameter(
          "Verb",
          Client.StringInput.Wildcard
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
        _ = verbDictionary.TryAdd(
          verbObject.Verb,
          verbObject
        );
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
