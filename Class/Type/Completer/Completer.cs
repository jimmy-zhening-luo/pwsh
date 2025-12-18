using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace Completer
{
  public enum CompletionCase
  {
    Preserve,
    Lower,
    Upper
  }

  public abstract class CompleterBase(
    CompletionCase Casing,
    bool Surrounding
  ) : IArgumentCompleter
  {
    private static IEnumerable<CompletionResult> WrapCompletionResult(
      IEnumerable<string> completedStrings
    )
    {
      foreach (string completedString in completedStrings)
      {
        yield return new CompletionResult(
          Typed.Typed.Escape(
            completedString
          )
        );
      }
    }

    public abstract IEnumerable<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    );

    public IEnumerable<CompletionResult> CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    )
    {
      return WrapCompletionResult(
        FulfillCompletion(
          parameterName,
          wordToComplete,
          fakeBoundParameters
        )
      );
    }

    public IEnumerable<string> FindCompletion(
      string wordToComplete,
      IEnumerable<string> domain
    )
    {
      string unescapedWordToComplete = Typed.Typed.Unescape(wordToComplete);

      if (
        string.IsNullOrWhiteSpace(
          unescapedWordToComplete
        )
      )
      {
        foreach (string member in domain)
        {
          yield return member;
        }

        yield break;
      }
      else
      {
        int matched = 0;

        foreach (string member in domain)
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
          foreach (string member in domain)
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
  }

  public class Completer : CompleterBase
  {
    public readonly IEnumerable<string> Domain;

    public Completer(
      IEnumerable<string> domain,
      CompletionCase casing,
      bool surrounding
    ): base(
      casing,
      surrounding
    )
    {
      Domain = domain;
    }

    public override IEnumerable<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      return FindCompletion(
        wordToComplete,
        Domain
      );
    }
  }

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
          .Split(",")
          .Select(
            member => member.Trim()
          ),
        Casing ?? CompletionCase.Preserve,
        Surrounding ?? true
      );
    }
  }

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
        Casing ?? CompletionCase.Preserve,
        Surrounding ?? true
      );
    }
  }
}
