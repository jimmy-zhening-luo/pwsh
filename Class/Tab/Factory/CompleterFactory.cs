namespace PowerModule.Tab.Factory;

abstract class CompleterFactory<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : Intrinsics.TCompleterFactory(Case)
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionResultType.ParameterValue;

  abstract private protected IEnumerable<string> EnumerateDomain(TDomain domain);

  sealed override public Completers.Completer Create() => new(
    CompletionType,
    Case,
    EnumerateDomain(Domain)
  );
}
