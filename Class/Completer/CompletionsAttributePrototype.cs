namespace Module.Completer;

public abstract class CompletionsAttributePrototype<TDomain>(
  TDomain Domain,
  bool Strict = false,
  CompletionCase casing = CompletionCase.Preserve
) : BaseCompletionsAttribute<Completer>(
  casing
)
{
  private protected abstract IEnumerable<string> ResolveDomain(
    TDomain domain
  );

  public sealed override Completer Create() => new(
    ResolveDomain(
      Domain
    ),
    Strict,
    Casing
  );
}
