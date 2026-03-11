namespace PowerModule.Tab.Completers;

sealed class SetCompleter(
  CompletionResultType CompletionType,
  CompletionCase Case,
  IEnumerable<string> Domain
) : Intrinsics.TCompleter(CompletionType, Case)
{
  sealed override private protected IEnumerable<Intrinsics.ICompleter.CompletionResultRecord> GenerateCompletion(string wordToComplete)
  {
    uint startingCount = default;

    foreach (var member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          System.StringComparison.OrdinalIgnoreCase
        )
      )
      {
        ++startingCount;

        yield return new(member);
      }
    }

    if (startingCount is 1)
    {
      var wordSize = wordToComplete.Length;

      foreach (var member in Domain)
      {
        if (
          member.Length > wordSize
          && member.IndexOf(
            wordToComplete,
            1,
            System.StringComparison.OrdinalIgnoreCase
          ) > 0
        )
        {
          yield return new(member);
        }
      }
    }

    yield break;
  }
}
