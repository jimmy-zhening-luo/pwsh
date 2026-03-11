namespace PowerModule.Tab;

sealed class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Case = CompletionCase.Lower
) : Factory.SetCompleterFactory<System.Type>(
  EnumType,
  Case
)
{
  public System.Type EnumType
  { get; } = EnumType;

  public string[]? Include
  { get; init; }

  public string[]? Exclude
  { get; init; }

  sealed override private protected IEnumerable<string> EnumerateDomain(System.Type enumType)
  {
    var names = System.Enum.GetNames(enumType);

    if (Exclude is null or [])
    {
      foreach (var name in names)
      {
        yield return name;
      }
    }
    else
    {
      HashSet<string> exclusions = [.. Exclude];

      foreach (var name in names)
      {
        if (!exclusions.Contains(name))
        {
          yield return name;
        }
      }
    }

    if (Include is not null)
    {
      foreach (var inclusion in Include)
      {
        yield return inclusion;
      }
    }

    yield break;
  }
}
