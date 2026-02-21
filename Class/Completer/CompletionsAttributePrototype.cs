namespace Module.Completer;

public abstract class CompletionsAttributePrototype<TDomain> : BaseCompletionsAttribute<Completer>
{
  private protected readonly TDomain Domain;

  private readonly bool Strict;

  public CompletionsAttributePrototype(
    TDomain domain
  ) : base() => Domain = domain;

  public CompletionsAttributePrototype(
    TDomain domain,
    bool strict
  ) : this(
    domain
  ) => Strict = strict;

  public CompletionsAttributePrototype(
    TDomain domain,
    bool strict,
    CompletionCase casing
  ) : base(
    casing
  ) => (
    Domain,
    Strict
  ) = (
    domain,
    strict
  );

  private protected abstract IEnumerable<string> ResolveDomain();

  public sealed override Completer Create() => new(
    ResolveDomain(),
    Strict,
    Casing
  );
}
