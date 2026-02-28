namespace Module.Tab.Completer;

public class Completer(
  IEnumerable<string> Domain,
  CompletionCase Casing,
  bool Strict
) : TabCompleter(Casing)
{
  private protected override IEnumerable<string> GenerateCompletion(string wordToComplete)
  {
    if (wordToComplete is "")
    {
      foreach (var member in Domain)
      {
        yield return member;
      }

      yield break;
    }

    uint matches = default;

    foreach (var member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          System.StringComparison.OrdinalIgnoreCase
        )
      )
      {
        ++matches;
        yield return member;
      }
    }

    if (
      Strict
      || matches is not 1
    )
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
        ++matches;
        yield return member;
      }
    }

    yield break;
  }
}
