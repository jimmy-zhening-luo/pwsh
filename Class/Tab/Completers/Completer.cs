namespace PowerModule.Tab.Completers;

sealed class Completer(
  CompletionResultType CompletionType,
  CompletionCase Case,
  IEnumerable<string> Domain
) : Intrinsics.TCompleter(CompletionType, Case)
{
  sealed override private protected IEnumerable<Intrinsics.ICompleter.CompletionResultRecord> GenerateCompletion(string wordToComplete)
  {
    uint index = default;

    foreach (var member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          System.StringComparison.OrdinalIgnoreCase
        )
      )
      {
        ++index;
        yield return new(member);
      }
    }

    if (index is not 1)
    {
      yield break;
    }

    foreach (var member in Domain)
    {
      if (
        member.Length > wordToComplete.Length
        && member.IndexOf(
          wordToComplete,
          1,
          System.StringComparison.OrdinalIgnoreCase
        ) > 0
      )
      {
        ++index;
        yield return new(member);
      }
    }

    yield break;
  }
}
