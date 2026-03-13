namespace PowerModule.Tab;

sealed class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  public string[] Domain
  { get; } = Domain;
}
abstract class CompletionsAttribute<TCollection>(
  TCollection Domain) : Factory.SetCompleterFactory<TCollection>(Domain) where TCollection : ICollection<string>
{
  sealed override private protected ICollection<string> EvaluateDomain(TCollection Domain) => Domain;
}
