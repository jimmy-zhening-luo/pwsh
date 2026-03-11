namespace PowerModule.Tab.Completers.Intrinsics;

abstract class TCompleter(
  CompletionCase Case,
  CompletionResultType CompletionType
) : ICompleter
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionType;

  public CompletionCase Case
  { get; init; } = Case;

  abstract private protected IEnumerable<ICompleter.CompletionResultRecord> GenerateCompletion(string wordToComplete);
  IEnumerable<ICompleter.CompletionResultRecord> ICompleter.GenerateCompletion(string wordToComplete) => GenerateCompletion(wordToComplete);
}
