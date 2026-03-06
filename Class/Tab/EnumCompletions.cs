namespace Module.Tab;

internal class EnumCompletionsAttribute(
  System.Type EnumType,
  string[]? Include = default,
  string[]? Exclude = default,
  CompletionCase Case = CompletionCase.Lower
) : CompletionsAttribute<System.Type>(
  EnumType,
  Case
)
{
  private protected sealed override IEnumerable<string> EnumerateDomain(System.Type enumType)
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
