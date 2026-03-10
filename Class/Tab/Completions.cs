namespace PowerModule.Tab;

sealed internal class CompletionsAttribute(params string[] Domain) : Factory.CompletionsAttribute<string[]>(Domain)
{
  public string[] Domain
  { get; } = Domain;

  sealed override private protected IEnumerable<string> EnumerateDomain(string[] domain) => domain;
}
