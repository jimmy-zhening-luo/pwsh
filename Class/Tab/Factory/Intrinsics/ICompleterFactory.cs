namespace PowerModule.Tab.Factory.Intrinsics;

interface ICompleterFactory : IArgumentCompleterFactory
{
  CompletionResultType CompletionType
  { get; init; }

  CompletionCase Case
  { get; init; }

  new Completers.Intrinsics.ICompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create()
    => Create();
}
