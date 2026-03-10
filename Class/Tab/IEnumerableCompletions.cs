namespace PowerModule.Tab;

abstract internal class IEnumerableCompletionsAttribute(
  IEnumerable<string> Domain) : Factory.CompleterFactory<IEnumerable<string>>(Domain)
{
  sealed override private protected IEnumerable<string> EnumerateDomain(IEnumerable<string> Domain) => Domain;
}
