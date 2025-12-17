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
      List<string> ordinalDomain = Casing switch
      {
        CompletionCase.Upper => [
          .. domain.Select(
            member => member.ToUpperInvariant()
          )
        ],
        CompletionCase.Lower => [
          ..domain.Select(
            member => member.ToLowerInvariant()
          )
        ],
        _ => [.. domain],
      };

      List<string> completions = [];

      string typed = Typed.Typed.Unescape(wordToComplete);

      if (string.IsNullOrWhiteSpace(typed))
      {
        if (ordinalDomain.Count != 0)
        {
          completions.AddRange(ordinalDomain);
        }
      }
      else
      {
        completions.AddRange(
          ordinalDomain.Where(
            member => member.Equals(
              typed,
              StringComparison.OrdinalIgnoreCase
            )
          )
        );

        bool hasExactMatch = completions.Count != 0;

        completions.AddRange(
          ordinalDomain.Where(
            member => member.StartsWith(
                typed,
                StringComparison.Ordinal
              )
              && !member.Equals(
                typed,
                StringComparison.OrdinalIgnoreCase
              )
          )
        );

        if (Surrounding)
        {
          if (
            completions.Count == 0
            || completions.Count == 1 && hasExactMatch
          )
          {
            completions.InsertRange(
              0,
              ordinalDomain.Where(
                member => !member.StartsWith(typed)
                  && member.Contains(
                    typed,
                    StringComparison.OrdinalIgnoreCase
                  )
              )
            );
          }
        }
      }

      return completions;
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
