namespace PowerModule.Tab;

abstract class CompletionsAttribute<TCollection>(TCollection Domain) : Factory.SetCompleterFactory<TCollection>(Domain) where TCollection : ICollection<string>
{
  sealed override private protected ICollection<string> EvaluateDomain(TCollection Domain) => Domain;
}
