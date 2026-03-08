namespace Module.Tab.Completers;

internal sealed class Completer(
  IEnumerable<string> Domain,
  bool Strict,
  CompletionCase Case,
  CompletionResultType CompletionType
) : TCompleter<string>(Case, CompletionType)
{
  private protected sealed override IEnumerable<string> GenerateDomain() => Domain;

  private protected sealed override IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete)
  {
    uint index = default;

    if (wordToComplete is "")
    {
      foreach (var member in GenerateDomain())
      {
        ++index;
        yield return new(member);
      }

      yield break;
    }

    foreach (var member in GenerateDomain())
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

    foreach (var member in GenerateDomain())
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