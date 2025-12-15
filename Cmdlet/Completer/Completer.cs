using System;
using System.Collections;
using System.Collections.Generic;
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
      HashSet<string> unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      foreach (string member in domain)
      {
        unique.Add(member);
      }

      if (unique.Count == 0)
      {
        throw new ArgumentException("domain");
      }

      List<string> domainSet = new List<string>(unique);

      Domain = domainSet;
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

  public class TestCompletionsAttribute : ArgumentCompleterAttribute {

    private readonly List<string> Span;
    private readonly CompletionCase Case;
    private readonly bool Sort;
    private readonly bool Surrounding;

    public TestCompletionsAttribute(
      List<string> span,
      CompletionCase caseOption = CompletionCase.Lower,
      bool sort = false,
      bool surrounding = true
    ) : base(typeof (Completer))
    {
      Span = span;
      Case = caseOption;
      Sort = sort;
      Surrounding = surrounding;
    }

    public Completer Create() {
      List<string> cleanSpan = new List<string>();

      foreach (string member in Span) {
        string cleanedMember = member.Trim();

        if (cleanedMember != String.Empty) {
          cleanSpan.Add(cleanedMember)
        }
      }

      return new Completer(
        cleanSpan,
        Case,
        Sort,
        Surrounding
      );
    }
  }
}
