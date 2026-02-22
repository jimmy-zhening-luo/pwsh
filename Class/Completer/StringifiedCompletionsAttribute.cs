namespace Module.Completer;

public sealed class StringifiedCompletionsAttribute : CompletionsAttributePrototype
{
  private readonly string Domain;

  public StringifiedCompletionsAttribute(
    string domain
  ) : base() => Domain = domain;

  public StringifiedCompletionsAttribute(
    string domain,
    bool strict
  ) : base(
    strict
  ) => Domain = domain;

  public StringifiedCompletionsAttribute(
    string domain,
    bool strict,
    CompletionCase casing
  ) : base(
    strict,
    casing
  ) => Domain = domain;

  private protected sealed override IEnumerable<string> ResolveDomain() => Domain.Split(
    ',',
    System.StringSplitOptions.RemoveEmptyEntries
    | System.StringSplitOptions.TrimEntries
  );
}
