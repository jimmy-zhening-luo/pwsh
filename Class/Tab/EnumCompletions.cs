namespace PowerModule.Tab;

sealed internal class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Case = CompletionCase.Lower
) : CompletionsAttribute<System.Type>(
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
    HashSet<string> exclusions = Exclude is null
      ? new()
      : new(Exclude);

    foreach (
      var name in System.Enum.GetNames(
        enumType
      )
    )
    {
      if (!exclusions.Contains(name))
      {
        yield return name;
      }
    }

    if (Include is null)
    {
      yield break;
    }

    foreach (var inclusion in Include)
    {
      yield return inclusion;
    }

    yield break;
  }
}
