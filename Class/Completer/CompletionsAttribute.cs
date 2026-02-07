namespace Module.Completer;

public sealed class CompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  private readonly string StringifiedDomain;

  private readonly bool Strict;

  public CompletionsAttribute(
    string stringifiedDomain
  ) : base() => StringifiedDomain = stringifiedDomain;

  public CompletionsAttribute(
    string stringifiedDomain,
    bool strict
  ) : this(
    stringifiedDomain
  ) => Strict = strict;

  public CompletionsAttribute(
    string stringifiedDomain,
    bool strict,
    CompletionCase casing
  ) : base(
    casing
  ) => (
    StringifiedDomain,
    Strict
  ) = (
    stringifiedDomain,
    strict
  );

  public sealed override Completer Create() => new(
    StringifiedDomain
      .Split(
        ',',
        StringSplitOptions.RemoveEmptyEntries
        | StringSplitOptions.TrimEntries
      ),
    Strict,
    Casing
  );
}
