namespace Module.Completer
{
  using System.Collections.Generic;

  public class Completer : BaseCompleter
  {
    public readonly IStringEnumerable Domain;

    public readonly bool Strict;

    public Completer(
      IStringEnumerable domain,
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

    public override IStringEnumerable FulfillCompletion(
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
}
