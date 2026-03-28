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

  const string GetVerbCommand = $@"{StandardModule.Utility}\Get-Verb";

  [Parameter(Position = default)]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Verb
  {
    private get => [.. verbs];
    init
    {
      verbs.Clear();
      verbs.EnsureCapacity(
        value.Length
      );

      foreach (var verb in value)
      {
        _ = verbs.Add(
          verb.Contains(
            Client.StringInput.Wildcard,
            System.StringComparison.Ordinal
          )
            ? verb
            : verb.Length > 2
              ? $"{Client.StringInput.StringWildcard}{verb}{Client.StringInput.StringWildcard}"
              : $"{verb}{Client.StringInput.StringWildcard}"
        );
      }
    }
  }
  readonly HashSet<string> verbs = [];

  [Parameter(Position = 1)]
  [ValidateNotNullOrEmpty]
  public VerbGroup[] Group
  { private get; init; } = [];

  sealed override private protected void Postprocess()
  {
    if (
      Verb is []
      or [Client.StringInput.StringWildcard]
    )
    {
      _ = AddCommand(GetVerbCommand)
        .AddParameter(
          nameof(Verb),
          Client.StringInput.StringWildcard
        );

      if (Group is not [])
      {
        _ = AddParameter(
          nameof(Group),
          Group
        );
      }

      WriteObject(
        AddCommand(
          $@"{StandardModule.Utility}\Select-Object"
        )
          .AddParameter(
            "ExpandProperty",
            nameof(Verb)
          )
          .AddCommand(
            $@"{StandardModule.Utility}\Sort-Object"
          )
          .Invoke(),
        true
      );
    }
    else
    {
      SortedDictionary<string, VerbInfo> verbDictionary = [];

      _ = AddCommand(GetVerbCommand)
        .AddParameter(
          nameof(Verb),
          Verb
        );

      if (Group is not [])
      {
        _ = AddParameter(
          nameof(Group),
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
