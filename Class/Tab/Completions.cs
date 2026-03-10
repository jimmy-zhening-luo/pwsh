namespace PowerModule.Tab;

sealed internal class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  public string[] Domain
  { get; } = Domain;

  sealed override private protected IEnumerable<string> EnumerateDomain(string[] domain) => domain;
}
abstract internal class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : Factory.TCompleterFactory(Case)
{
  public bool Strict
  { get; init; }

  abstract private protected IEnumerable<string> EnumerateDomain(TDomain domain);

  sealed override public Completers.Completer Create() => new(
    EnumerateDomain(Domain),
    Strict,
    Case,
    CompletionType
  );
}
