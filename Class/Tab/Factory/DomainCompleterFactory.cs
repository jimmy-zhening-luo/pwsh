namespace PowerModule.Tab.Factory;

abstract class DomainCompleterFactory(ICollection<string> Domain) : DomainCompleterFactory<ICollection<string>>(Domain)
{
  sealed override private protected ICollection<string> EvaluateDomain(ICollection<string> Domain) => Domain;
}
abstract class DomainCompleterFactory<TDomain>(TDomain Domain) : Intrinsics.CompleterFactory
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionResultType.ParameterValue;

  abstract private protected ICollection<string> EvaluateDomain(TDomain domain);

  sealed override public Completers.DomainCompleter Create() => new(
    CompletionType,
    Casing,
    EvaluateDomain(Domain)
  );
}
