namespace Module.Tab;

internal class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  private protected sealed override IEnumerable<string> EnumerateDomain(string[] domain) => domain;
}
internal abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : TCompletionsAttribute(Case)
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
