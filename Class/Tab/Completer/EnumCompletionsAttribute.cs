namespace Module.Tab.Completer;

public class EnumCompletionsAttribute(
  System.Type EnumType,
  CompletionCase Casing = CompletionCase.Lower,
  bool Strict = false
) : CompletionsAttribute<System.Type>(
  EnumType,
  Casing,
  Strict
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    System.Type enumType
  ) => System.Enum.GetNames(
    enumType
  );
}
