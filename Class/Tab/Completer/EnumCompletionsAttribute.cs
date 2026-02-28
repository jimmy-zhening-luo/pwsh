namespace Module.Tab.Completer;

public class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Casing = CompletionCase.Lower,
  string[]? Exclude = default
) : CompletionsAttribute<System.Type>(
  EnumType,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(System.Type enumType)
  {
    var domain = System.Enum.GetNames(enumType);

    if (Exclude is null or [])
    {
      return domain;
    }
    else
    {
      var domainSet = new HashSet<string>(domain);

      domainSet.ExceptWith(Exclude);

      return domainSet;
    }
  }
}
