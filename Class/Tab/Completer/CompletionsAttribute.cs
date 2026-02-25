namespace Module.Tab.Completer;

public class CompletionsAttribute(
  string[] Domain,
  CompletionCase Casing = default
) : CompletionsAttribute<string[]>(
  Domain,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(
    string[] domain
  ) => domain;
}

public abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Casing
) : TabCompletionsAttribute<Completer>(
  Casing
)
{
  public bool Strict { get; init; }

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
