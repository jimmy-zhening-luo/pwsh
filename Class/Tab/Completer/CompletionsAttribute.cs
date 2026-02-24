namespace Module.Tab.Completer;

public class CompletionsAttribute(
  System.Type Domain,
  CompletionCase Casing = CompletionCase.Lower,
  bool Strict = false
) : CompletionsAttributePrototype<string[]>(
  Domain,
  Casing,
  Strict
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}
