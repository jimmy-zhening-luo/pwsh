namespace Module.Completer;

public sealed class CompletionsAttribute : CompletionsAttributePrototype
{
  private readonly string[] Domain;

  public CompletionsAttribute(
    string[] domain
  ) : base() => Domain = domain;

  public CompletionsAttribute(
    string[] domain,
    bool strict
  ) : base(
    strict
  ) => Domain = domain;

  public CompletionsAttribute(
    string[] domain,
    bool strict,
    CompletionCase casing
  ) : base(
    strict,
    casing
  ) => Domain = domain;

  private protected sealed override IEnumerable<string> ResolveDomain() => Domain;
}
