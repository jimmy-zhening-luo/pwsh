namespace Module.Completer;

public sealed class StringifiedCompletionsAttribute : CompletionsAttributePrototype<string>
{
  public StringifiedCompletionsAttribute(
    string domain
  ) : base(domain)
  { }

  public StringifiedCompletionsAttribute(
    string domain,
    bool strict
  ) : base(
    domain,
    strict
  )
  { }

  public StringifiedCompletionsAttribute(
    string domain,
    bool strict,
    CompletionCase casing
  ) : base(
    domain,
    strict,
    casing
  )
  { }

  private protected sealed override IEnumerable<string> ResolveDomain(
    string domain
  ) => domain.Split(
    ',',
    System.StringSplitOptions.RemoveEmptyEntries
    | System.StringSplitOptions.TrimEntries
  );
}
