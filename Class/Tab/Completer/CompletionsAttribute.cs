namespace Module.Tab.Completer;

public class CompletionsAttribute(
  string[] Domain,
  CompletionCase Casing = CompletionCase.Preserve
) : CompletionsAttribute<string[]>(
  Domain,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}
