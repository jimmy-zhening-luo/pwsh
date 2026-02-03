namespace Module.Completer;

public sealed class Completer : BaseCompleter
{
  private protected readonly IEnumerable<string> Domain;

  private protected readonly bool Strict;

  public Completer(
    IEnumerable<string> domain,
    bool strict,
    CompletionCase casing
  ) : base(
    casing
  ) => (
    Domain,
    Strict
  ) = (
    domain,
    strict
  );

  public sealed override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    if (
      string.IsNullOrEmpty(
        wordToComplete
      )
    )
    {
      foreach (string member in Domain)
      {
        yield return member;
      }

      yield break;
    }

    int count = 0;
    foreach (string member in Domain)
    {
      if (
        member.StartsWith(
          wordToComplete,
          StringComparison.OrdinalIgnoreCase
        )
      )
      {
        ++count;
        yield return member;
      }
    }

    if (
      Strict
      || count > 1
    )
    {
      yield break;
    }

    foreach (string member in Domain)
    {
      if (
        member.Length > wordToComplete.Length
        && member.IndexOf(
          wordToComplete,
          1,
          StringComparison.OrdinalIgnoreCase
        ) > 0
      )
      {
        yield return member;
      }
    }
  }
}
