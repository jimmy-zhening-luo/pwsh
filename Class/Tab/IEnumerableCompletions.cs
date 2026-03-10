namespace PowerModule.Tab.Generic;

abstract internal class IEnumerableCompletionsAttribute(
  IEnumerable<string> Domain) : CompletionsAttribute<IEnumerable<string>>(Domain)
{
  sealed override private protected IEnumerable<string> EnumerateDomain(IEnumerable<string> Domain) => domain;
}
