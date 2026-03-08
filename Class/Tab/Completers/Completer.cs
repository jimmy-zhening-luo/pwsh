namespace Module.Tab.Completers;

sealed internal class Completer(
  IEnumerable<string> Domain,
  bool Strict,
  CompletionCase Case,
  CompletionResultType CompletionType
) : TCompleter(Case, CompletionType)
{
  sealed override private protected IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete)
  {
    uint index = default;

    if (wordToComplete is "")
    {
      foreach (var member in Domain)
      {
        ++index;
        yield return new(member);
      }

      yield break;
    }

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

    if (Strict || index is not 1)
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
