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
  private protected sealed override IEnumerable<string> ResolveDomain(System.Type enumType) => Exclude is null
    ? System.Enum.GetNames(enumType)
    : new HashSet<string>(
        System.Enum.GetNames(enumType)
      )
        .ExceptWith(Exclude);
}
