namespace PowerModule.Tab.Completers;

sealed class SetCompleter(
  CompletionResultType CompletionType,
  CompletionCase Case,
  IEnumerable<string> Domain
) : Intrinsics.TCompleter(CompletionType, Case)
{
  sealed override private protected IEnumerable<Intrinsics.ICompleter.CompletionResultRecord> GenerateCompletion(string wordToComplete)
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
