namespace Module.Tab.Completer;

public class CompletionsAttribute(
  string[] Domain,
  CompletionCase Casing = CompletionCase.Preserve,
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
