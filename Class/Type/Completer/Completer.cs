using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class DynamicCompletionsAttribute(
    ScriptBlock DomainGenerator,
    CompletionCase? Casing,
    bool? Surrounding
  ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public IArgumentCompleter Create()
    {
      return new Completer(
        DomainGenerator
          .Invoke()
          .Select(
            member => member
              .BaseObject
              .ToString()
          ),
        Surrounding,
        Casing
      );
    }
  } // class DynamicCompletionsAttribute

  [AttributeUsage(AttributeTargets.Parameter)]
  public class StaticCompletionsAttribute(
    string StringifiedDomain,
    CompletionCase? Casing,
    bool? Surrounding
  ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public IArgumentCompleter Create()
    {
      return new Completer(
        StringifiedDomain
          .Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries
            | StringSplitOptions.TrimEntries
          ),
        Surrounding,
        Casing
      );
    }
  } // class StaticCompletionsAttribute

  internal class Completer : CompleterBase
  {
    public readonly IEnumerable<string> Domain;
    public readonly bool Surrounding = true;

    public Completer(
      IEnumerable<string> domain
    )
    {
      Domain = domain;
    }

    public Completer(
      IEnumerable<string> domain,
      bool surrounding
    )
    {
      Domain = domain;
      Surrounding = surrounding;
    }

    public Completer(
      IEnumerable<string> domain,
      bool surrounding,
      CompletionCase casing
    ) : base(casing)
    {
      Domain = domain;
      Surrounding = surrounding;
    }

    public IEnumerable<string> FindCompletion(
      string wordToComplete
    )
    {
      string unescapedWordToComplete = Stringifier.Unescape(wordToComplete);
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
        if (Surrounding && matched <= 1)
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

    protected override IEnumerable<string> FulfillArgumentCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      return FindCompletion(wordToComplete);
    }
  } // class Completer
} // namespace Completer
