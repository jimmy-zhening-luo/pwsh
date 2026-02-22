namespace Module.Completer;

public abstract class CompletionsAttributePrototype<T>(
  T Domain,
  bool Strict = false,
  CompletionCase casing = CompletionCase.Preserve
) : BaseCompletionsAttribute<Completer>(
  casing
)
{
  private protected abstract IEnumerable<string> ResolveDomain(
    T domain
  );

  public sealed override Completer Create() => new(
    ResolveDomain(Domain),
    Strict,
    Casing
  );
}
