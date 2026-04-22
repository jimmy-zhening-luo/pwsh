namespace PowerModule.Tab.Completers;

sealed class DomainCompleter(
  CompletionResultType CompletionType,
  CompletionCase Casing,
  ICollection<string> Domain
) : Intrinsics.Completer(CompletionType, Casing)
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
