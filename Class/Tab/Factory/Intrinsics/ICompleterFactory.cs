namespace PowerModule.Tab.Factory.Intrinsics;

interface ICompleterFactory : IArgumentCompleterFactory
{
  CompletionCase Case
  { get; init; }

  CompletionResultType CompletionType
  { get; init; }

  new Completers.Intrinsics.ICompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create()
    => Create();
}
