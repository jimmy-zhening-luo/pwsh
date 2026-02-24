namespace Module.Tab.Completer;

public class CompletionsAttribute(
  string[] Domain,
  CompletionCase Casing = default
) : CompletionsAttribute<string[]>(
  Domain,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}
