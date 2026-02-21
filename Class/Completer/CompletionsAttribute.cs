namespace Module.Completer;

public sealed class CompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  private readonly string[] Domain;

  private readonly bool Strict;

  public CompletionsAttribute(
    string[] domain
  ) : base() => Domain = domain;

  public CompletionsAttribute(
    string[] domain,
    bool strict
  ) : this(
    domain
  ) => Strict = strict;

  public CompletionsAttribute(
    string[] domain,
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

  public sealed override Completer Create() => new(
    Domain,
    Strict,
    Casing
  );
}
