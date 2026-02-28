namespace Module.Tab.Completer;

internal class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Casing = CompletionCase.Lower,
  string[]? Include = default,
  string[]? Exclude = default
) : CompletionsAttribute<System.Type>(
  EnumType,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(System.Type enumType)
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
