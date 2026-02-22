namespace Module.Completer;

public class CompletionsAttribute : CompletionsAttributePrototype<string[]>
{
  public CompletionsAttribute(
    string[] domain
  ) : base(
    domain
  )
  { }

  public CompletionsAttribute(
    string[] domain,
    bool strict
  ) : base(
    domain,
    strict
  )
  { }

  public CompletionsAttribute(
    string[] domain,
    bool strict,
    CompletionCase casing
  ) : base(
    domain,
    strict,
    casing
  )
  { }

  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}
