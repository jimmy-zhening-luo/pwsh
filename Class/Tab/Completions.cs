namespace Module.Tab;

internal class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  private protected sealed override IEnumerable<string> EnumerateDomain(string[] domain) => domain;
}
internal abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : TCompletionsAttribute<Completers.Completer>(Case)
{
  public bool Strict { get; init; }

  private protected abstract IEnumerable<string> EnumerateDomain(TDomain domain);

  public sealed override Completers.Completer Create() => new(
    EnumerateDomain(Domain),
    Strict,
    Case,
    CompletionType
  );
}

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]

internal abstract class TCompletionsAttribute<T>(
  CompletionCase Case = default,
  CompletionResultType CompletionType = CompletionResultType.ParameterValue
) : ArgumentCompleterAttribute, IArgumentCompleterFactory where T : Completers.TCompleter
{
  public CompletionCase Case { get; init; } = Case;

  public CompletionResultType CompletionType { get; init; } = CompletionType;

  public abstract T Create<T>();
  public IArgumentCompleter Create() => Create<T>();
}
