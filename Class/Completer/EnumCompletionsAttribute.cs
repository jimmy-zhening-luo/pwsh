namespace Module.Completer;

public sealed class EnumCompletionsAttribute : CompletionsAttributePrototype<System.Type>
{
  private protected sealed override IEnumerable<string> ResolveDomain() => System.Enum.GetNames(
    Domain
  );
}
