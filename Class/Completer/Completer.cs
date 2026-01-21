namespace Module.Completer;

public class Completer : BaseCompleter
{
  public readonly IEnumerable<string> Domain;

  public readonly bool Strict;

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

  public override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    if (string.IsNullOrEmpty(wordToComplete))
    {
      foreach (string member in Domain)
      {
        yield return member;
      }

      yield break;
    }
    else
    {
      int matched = 0;
      foreach (string member in Domain)
      {
        if (
          member.StartsWith(
            wordToComplete,
            StringComparison.OrdinalIgnoreCase
          )
        )
        {
          ++matched;
          yield return member;
        }
      }

      if (
        !Strict
        && matched < 2
      )
      {
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

      yield break;
    }
  }
}
