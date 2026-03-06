namespace Module.Tab;

internal class EnumCompletionsAttribute(
  System.Type EnumType,
  string[]? Include = default,
  string[]? Exclude = default,
  CompletionCase Case = CompletionCase.Lower
) : CompletionsAttribute<System.Type>(EnumType, Case)
{
  private protected sealed override IEnumerable<string> CreateDomain(System.Type enumType)
  {
    var domain = new HashSet<string>(
      System.Enum.GetNames(enumType)
    );

    if (Include is [_, ..])
    {
      domain.UnionWith(Include);
    }
    if (Exclude is [_, ..])
    {
      domain.ExceptWith(Exclude);
    }

    return domain;
  }
}
