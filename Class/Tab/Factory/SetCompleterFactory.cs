namespace PowerModule.Tab.Factory;

abstract class SetCompleterFactory<TDomain>(TDomain Domain) : Intrinsics.TCompleterFactory
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionResultType.ParameterValue;

  abstract private protected ICollection<string> EvaluateDomain(TDomain domain);

  sealed override public Completers.SetCompleter Create() => new(
    CompletionType,
    Case,
    EvaluateDomain(Domain)
  );
}
