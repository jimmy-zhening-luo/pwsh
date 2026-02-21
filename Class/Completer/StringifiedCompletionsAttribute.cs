namespace Module.Completer;

public sealed class StringifiedCompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  private readonly string StringifiedDomain;

  private readonly bool Strict;

  public StringifiedCompletionsAttribute(
    string stringifiedDomain
  ) : base() => StringifiedDomain = stringifiedDomain;

  public StringifiedCompletionsAttribute(
    string stringifiedDomain,
    bool strict
  ) : this(
    stringifiedDomain
  ) => Strict = strict;

  public StringifiedCompletionsAttribute(
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
        System.StringSplitOptions.RemoveEmptyEntries
        | System.StringSplitOptions.TrimEntries
      ),
    Strict,
    Casing
  );
}
