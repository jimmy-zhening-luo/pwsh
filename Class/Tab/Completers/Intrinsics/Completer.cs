namespace PowerModule.Tab.Completers.Intrinsics;

abstract class Completer(
  CompletionResultType CompletionType,
  CompletionCase Casing
) : ICompleter
{
  public CompletionResultType CompletionType
  { get; init; } = CompletionType;

  public CompletionCase Casing
  { get; init; } = Casing;

  abstract private protected IEnumerable<ICompleter.Completion> GenerateCompletion(string wordToComplete);
  IEnumerable<ICompleter.Completion> ICompleter.GenerateCompletion(string wordToComplete) => GenerateCompletion(wordToComplete);
}
