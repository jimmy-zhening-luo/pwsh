namespace PowerModule.Tab.Factory.Intrinsics;

interface ICompleterFactory : IArgumentCompleterFactory
{
  CompletionCase Casing
  { get; init; }

  new Completers.Intrinsics.ICompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create()
    => Create();
}
