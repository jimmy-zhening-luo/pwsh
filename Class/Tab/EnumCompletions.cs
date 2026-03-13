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

  sealed override private protected IEnumerable<string> EnumerateDomain(System.Type enumType)
  {
    HashSet<string> names = [.. System.Enum.GetNames(enumType)];

    if (Exclude is not null or [])
    {
      names.ExceptWith(Exclude);
    }

    if (Include is not null)
    {
      foreach (var inclusion in Include)
      {
        _ = names.Add(inclusion);
      }
    }

    return names;
  }
}
