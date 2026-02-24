namespace Module.Tab.Completer;

public sealed class Completer(
  IEnumerable<string> Domain,
  CompletionCase Casing,
  bool Strict
) : TabCompleter(
  Casing
)
{
  private protected sealed override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    if (
      string.IsNullOrEmpty(
        wordToComplete
      )
    )
    {
      foreach (var member in Domain)
      {
        yield return member;
      }

      yield break;
    }

    int count = default;

    foreach (var member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          System.StringComparison.OrdinalIgnoreCase
        )
      )
      {
        ++count;
        yield return member;
      }
    }

    if (
      Strict
      || count != 1
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
        yield return member;
      }
    }
  }
}
