namespace PowerModule.Tab;

sealed class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  public string[] Domain
  { get; } = Domain;
}
abstract class CompletionsAttribute<TEnumerable>(
  TEnumerable Domain) : Factory.CompleterFactory<TEnumerable>(Domain) where TEnumerable : IEnumerable<string>
{
  sealed override private protected IEnumerable<string> EnumerateDomain(TEnumerable Domain) => Domain;
}
