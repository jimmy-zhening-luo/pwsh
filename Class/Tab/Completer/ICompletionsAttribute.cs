namespace Module.Tab.Completer;

public abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Casing,
  bool Strict
) : TabCompletionsAttribute<Completer>(
  Casing
)
{
  private protected abstract IEnumerable<string> ResolveDomain(
    TDomain domain
  );

  public sealed override Completer Create() => new(
    ResolveDomain(
      Domain
    ),
    Casing,
    Strict
  );
}
