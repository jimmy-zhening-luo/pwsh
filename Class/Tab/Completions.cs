namespace PowerModule.Tab;

sealed class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  public string[] Domain
  { get; } = Domain;
}
abstract class CompletionsAttribute<TEnumerable>(
  TEnumerable Domain) : Factory.SetCompleterFactory<TEnumerable>(Domain) where TEnumerable : ICollection<string>
{
  sealed override private protected ICollection<string> EvaluateDomain(TEnumerable Domain) => Domain;
}
