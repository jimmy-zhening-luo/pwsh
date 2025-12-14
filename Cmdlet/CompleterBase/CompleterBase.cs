using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace CompleterBase
{
  public enum CompletionCase
  {
    Preserve,
    Lower,
    Upper
  }

  public abstract class CompleterBase : IArgumentCompleter
  {
    public static string Unescape(string escapedText)
    {
      return (
        escapedText.Length > 1
        && escapedText.StartsWith("'")
        && escapedText.EndsWith("'")
      )
        ? escapedText.Substring(
            1,
            escapedText.Length - 2
          ).Replace(
            "''",
            "'"
          )
        : escapedText;
    }

    public static string Escape(string text)
    {
      return text.Contains(" ")
        ? "'" + CodeGeneration.EscapeSingleQuotedStringContent(text) + "'"
        : text;
    }

    public static List<string> FindCompletion(
      string wordToComplete,
      List<string> domain,
      CompletionCase caseOption = CompletionCase.Preserve,
      bool sort = false,
      bool surrounding = false
    )
    {
      List<string> completions = new List<string>();
      List<string> domainCased = new List<string>();

      if (domain.Count != 0)
      {
        switch (caseOption)
        {
          case CompletionCase.Upper:
            foreach (string member in domain)
            {
              domainCased.Add(member.ToUpperInvariant());
            }
            break;
          case CompletionCase.Lower:
            foreach (string member in domain)
            {
              domainCased.Add(member.ToLowerInvariant());
            }
            break;
          default:
            domainCased.AddRange(domain);
            break;
        }
      }

      if (sort)
      {
        domainCased.Sort();
      }

      string typed = Unescape(wordToComplete);

      if (!string.IsNullOrWhiteSpace(typed))
      {
        foreach (string member in domainCased)
        {
          if (member.StartsWith(typed, StringComparison.OrdinalIgnoreCase))
          {
            completions.Add(member);
          }
        }

        if (
          surrounding
          && (
            completions.Count == 0
            || (
              completions.Count == 1
              && completions[0].Equals(
                typed,
                StringComparison.OrdinalIgnoreCase
              )
            )
          )
        )
        {
          foreach (string member in domainCased)
          {
            if (
              member.IndexOf(typed, StringComparison.OrdinalIgnoreCase) >= 0
              && !member.Equals(typed, StringComparison.OrdinalIgnoreCase)
            )
            {
              completions.Add(member);
            }
          }
        }
      }
      else
      {
        if (domainCased.Count != 0)
        {
          completions.AddRange(domainCased);
        }
      }

      return completions;
    }

    public static List<CompletionResult> CreateCompletionResult(
      List<string> completions
    )
    {
      List<CompletionResult> completionResults = new List<CompletionResult>();

      foreach (string completion in completions)
      {
        completionResults.Add(
          new CompletionResult(
            Escape(
              completion
            )
          )
        );
      }

      return completionResults;
    }

    public abstract List<string> FulfillCompletion(
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
      return CreateCompletionResult(
        FulfillCompletion(
          parameterName,
          wordToComplete,
          fakeBoundParameters
        )
      );
    }
  }

  public class Completer : CompleterBase
  {
    private readonly List<string> Domain;
    private readonly CompletionCase Case;
    private readonly bool Sort;
    private readonly bool Surrounding;

    public Completer(
      List<string> domain,
      CompletionCase caseOption,
      bool sort,
      bool surrounding
    )
    {
      if (domain.Count == 0)
      {
        throw new ArgumentException("domain");
      }

      HashSet<string> unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      foreach (string unit in domain)
      {
        unique.Add(unit);
      }

      if (unique.Count != domain.Count)
      {
        throw new ArgumentException("domain");
      }

      Domain = domain;
      Case = caseOption;
      Sort = sort;
      Surrounding = surrounding;
    }

    public override List<string> FulfillCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    )
    {
      return FindCompletion(
        wordToComplete,
        Domain,
        Case,
        Sort,
        Surrounding
      );
    }
  }
}
