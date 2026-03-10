namespace PowerModule.Tab.Completers.Intrinsics;

abstract internal class TCompleter(
  CompletionCase Case,
  CompletionResultType CompletionType
) : ICompleter
{
  public CompletionCase Case { get; init; } = Case;
  public CompletionResultType CompletionType { get; init; } = CompletionType;

  abstract private protected IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete);
  IEnumerable<CompletionResultRecord> ICompleter.GenerateCompletion(string wordToComplete) => GenerateCompletion(wordToComplete);
}
