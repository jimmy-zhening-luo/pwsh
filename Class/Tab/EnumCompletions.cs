namespace PowerModule.Tab;

sealed internal class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Case = CompletionCase.Lower
) : Factory.CompleterFactory<System.Type>(
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
      foreach (var name in names he)
      {
        yield return name;
      }
    }
    else
    {
      HashSet<string> exclusions = new(Exclude);

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
