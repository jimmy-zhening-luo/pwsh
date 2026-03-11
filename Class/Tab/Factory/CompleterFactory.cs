namespace PowerModule.Tab.Factory;

abstract class CompleterFactory<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : Intrinsics.TCompleterFactory(Case)
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionType;

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
