using System;
using System.Collections.Generic;

namespace Completer
{
  public class Completer : BaseCompleter
  {
    public readonly IEnumerable<string> Domain;
    public readonly bool Strict;

    private Completer() : base() { }

    public Completer(
      IEnumerable<string> domain,
      bool strict,
      CompletionCase casing
    ) : base(casing) => (Domain, Strict) = (
      domain,
      strict
    );

    public override IEnumerable<string> FulfillCompletion(string wordToComplete)
    {
      if (wordToComplete == string.Empty)
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
        if (!Strict && matched <= 1)
        {
          foreach (string member in Domain)
          {
            if (
              member.Length > wordToComplete.Length
              && member.IndexOf(
                wordToComplete,
                1,
                StringComparison.OrdinalIgnoreCase
              ) >= 1
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
