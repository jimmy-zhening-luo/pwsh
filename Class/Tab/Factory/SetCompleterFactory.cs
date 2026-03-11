namespace PowerModule.Tab.Factory;

abstract class SetCompleterFactory<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : Intrinsics.TCompleterFactory(Case)
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionResultType.ParameterValue;

  abstract private protected IEnumerable<string> EnumerateDomain(TDomain domain);

  sealed override public Completers.SetCompleter Create() => new(
    CompletionType,
    Case,
    EnumerateDomain(Domain)
  );
}
