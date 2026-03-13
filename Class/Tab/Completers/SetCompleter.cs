namespace PowerModule.Tab.Completers;

sealed class SetCompleter(
  CompletionResultType CompletionType,
  CompletionCase Case,
  ICollection<string> Domain
) : Intrinsics.Completer(CompletionType, Case)
{
  sealed override private protected IEnumerable<Intrinsics.ICompleter.Completion> GenerateCompletion(string wordToComplete)
  {
    foreach (var member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          System.StringComparison.OrdinalIgnoreCase
        )
      )
      {
        yield return new(member);
      }
    }

    yield break;
  }
}
