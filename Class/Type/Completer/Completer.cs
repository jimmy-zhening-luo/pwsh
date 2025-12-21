using System;
using System.Collections;
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
    ) : base(casing)
    {
      Domain = domain;
      Strict = strict;
    }

    public IEnumerable<string> FindCompletion(
      string wordToComplete
    )
    {
      string unescapedWordToComplete = Escaper.Unescape(wordToComplete);
      if (
        string.IsNullOrWhiteSpace(
          unescapedWordToComplete
        )
      )
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
              unescapedWordToComplete,
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
              member.Length > unescapedWordToComplete.Length
              && member.IndexOf(
                unescapedWordToComplete,
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

    public override IEnumerable<string> FulfillArgumentCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    ) => FindCompletion(wordToComplete);
  }
}
