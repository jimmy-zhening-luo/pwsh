namespace Module.Tab.Completer;

public class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Casing = CompletionCase.Lower
) : CompletionsAttribute<System.Type>(
  EnumType,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(System.Type enumType) => System.Enum.GetNames(enumType);
}
