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
    var names = System.Enum.GetNames(
      enumType
    );

    List<string> domain = [];

    if (Exclude is null or [])
    {
      domain.AddRange(names);
    }
    else
    {
      HashSet<string> exclusions = [.. Exclude];

      foreach (var name in names)
      {
        if (!exclusions.Contains(name))
        {
          domain.Add(name);
        }
      }
    }



    if (Include is not null)
    {
      domain.AddRange(Include);
    }

    return domain;
  }
}
