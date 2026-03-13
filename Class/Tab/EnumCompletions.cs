namespace PowerModule.Tab;

sealed class EnumCompletionsAttribute(System.Type EnumType) : Factory.SetCompleterFactory<System.Type>(EnumType)
{
  sealed override public CompletionCase Case
  { get; init; } = CompletionCase.Lower;

  public System.Type EnumType
  { get; } = EnumType;

  public string[]? Include
  { get; init; }

  public string[]? Exclude
  { get; init; }

  sealed override private protected ICollection<string> EvaluateDomain(System.Type enumType)
  {
    List<string> domain = [];
    HashSet<string> exclusions = Exclude is null
      ? []
      : [.. Exclude];

    foreach (
      var name in System.Enum.GetNames(
        enumType
      )
    )
    {
      if (!exclusions.Contains(name))
      {
        domain.Add(name);
      }
    }

    if (Include is not null)
    {
      domain.AddRange(Include);
    }

    return domain;
  }
}
