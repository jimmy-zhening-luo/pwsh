namespace Module.Completer;

public sealed class RestCompletionsAttribute(
  params string[] domain
) : CompletionsAttributePrototype<string[]>(
  domain
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}
