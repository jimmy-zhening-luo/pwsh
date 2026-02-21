namespace Module.Completer;

public sealed class CompletionsAttribute : CompletionsAttributePrototype<string[]>
{
  private protected sealed override IEnumerable<string> ResolveDomain() => Domain;
}
