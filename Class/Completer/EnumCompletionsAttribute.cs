namespace Module.Completer;

public sealed class EnumCompletionsAttribute(
  System.Type enumType,
  bool strict,
  CompletionCase casing
) : CompletionsAttributePrototype<System.Type>(
  enumType,
  strict,
  casing
)
{
  public EnumCompletionsAttribute(
    System.Type enumType
  ) : this(
    enumType,
    false
  )
  { }

  public EnumCompletionsAttribute(
    System.Type enumType,
    bool strict
  ) : this(
    enumType,
    strict,
    CompletionCase.Lower
  )
  { }

  private protected sealed override IEnumerable<string> ResolveDomain(
    System.Type enumType
  ) => System.Enum.GetNames(
    enumType
  );
}
