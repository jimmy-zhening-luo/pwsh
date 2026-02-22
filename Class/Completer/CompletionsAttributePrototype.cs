namespace Module.Completer;

public abstract class CompletionsAttributePrototype(
  bool Strict = false,
  CompletionCase casing = CompletionCase.Preserve
) : BaseCompletionsAttribute<Completer>(
  casing
)
{
  private protected abstract IEnumerable<string> ResolveDomain();

  public sealed override Completer Create() => new(
    ResolveDomain(),
    Strict,
    Casing
  );
}
