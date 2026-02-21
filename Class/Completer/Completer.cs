namespace Module.Completer;

public sealed class Completer : BaseCompleter<string>
{
  private readonly IEnumerable<string> Domain;

  private readonly bool Strict;

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
      foreach (var member in Domain)
      {
        yield return member;
      }

      yield break;
    }

    int count = 0;
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
      || count > 1
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
