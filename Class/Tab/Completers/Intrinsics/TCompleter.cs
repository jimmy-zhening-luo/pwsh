namespace PowerModule.Tab.Completers.Intrinsics;

abstract internal class TCompleter(
  CompletionCase Case,
  CompletionResultType CompletionType
) : ICompleter
{
  public CompletionCase Case
  { get; init; } = Case;
  public CompletionResultType CompletionType
  { get; init; } = CompletionType;

  abstract private protected IEnumerable<ICompleter.CompletionResultRecord> GenerateCompletion(string wordToComplete);
  IEnumerable<ICompleter.CompletionResultRecord> ICompleter.GenerateCompletion(string wordToComplete) => GenerateCompletion(wordToComplete);
}
