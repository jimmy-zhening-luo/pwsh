namespace PowerModule.Tab.Factory;

abstract internal class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : TCompletionsAttribute(Case)
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
