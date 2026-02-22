namespace Module.Completer;

public sealed class EnumCompletionsAttribute : CompletionsAttributePrototype
{
  private readonly System.Type Domain;

  public EnumCompletionsAttribute(
    System.Type domain
  ) : this(
    domain,
    false,
    CompletionCase.Lower
  )
  { }

  public EnumCompletionsAttribute(
    System.Type domain,
    bool strict
  ) : this(
    domain,
    strict,
    CompletionCase.Lower
  )
  { }

  public EnumCompletionsAttribute(
    System.Type domain,
    bool strict,
    CompletionCase casing
  ) : base(
    strict,
    casing
  ) => Domain = domain;

  private protected sealed override IEnumerable<string> ResolveDomain() => System.Enum.GetNames(
    Domain
  );
}
