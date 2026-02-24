namespace Module.Tab.Completer;

public abstract class CompletionsAttributePrototype<TDomain>(
  TDomain Domain,
  CompletionCase Casing = CompletionCase.Preserve,
  bool Strict = false
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
