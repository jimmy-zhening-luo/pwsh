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

  public abstract class CompleterBase : IArgumentCompleter
  {
    public static List<string> FindCompletion(
      string wordToComplete,
      List<string> domain,
      CompletionCase caseOption = CompletionCase.Preserve,
      bool sort = false,
      bool surrounding = false
    )
    {
      List<string> ordinalDomain = domain.Count == 0 ? [] : caseOption switch
      {
        CompletionCase.Upper => [
          ..domain.Select(member => member.ToUpperInvariant())
        ],
        CompletionCase.Lower => [
          ..domain.Select(member => member.ToLowerInvariant())
        ],
        _ => domain,
      };

      if (sort)
      {
        ordinalDomain.Sort();
      }

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

        if (surrounding)
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

    public static List<CompletionResult> CreateCompletionResult(
      List<string> completions
    )
    {
      List<CompletionResult> completionResults = [];

      foreach (string completion in completions)
      {
        completionResults.Add(
          new CompletionResult(
            Typed.Typed.Escape(
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
    public readonly List<string> Domain;
    public readonly CompletionCase Case;
    public readonly bool Sort;
    public readonly bool Surrounding;

    public Completer(
      List<string> span,
      CompletionCase casing,
      bool sort,
      bool surrounding
    )
    {
      HashSet<string> set = new
      (
        span
          .Select(s => s.Trim())
          .Where(
            trimmedCandidate => trimmedCandidate != string.Empty
          ),
        StringComparer.OrdinalIgnoreCase
      );

      if (set.Count == 0)
      {
        throw new ArgumentException("domain");
      }

      Domain = [.. set];
      Case = casing;
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

  [AttributeUsage(AttributeTargets.Parameter)]
  public class StaticCompletionsAttribute(
      string Units,
      CompletionCase? Casing,
      bool? Sort,
      bool? Surrounding
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public IArgumentCompleter Create()
    {
      return new Completer(
          [.. Units.Split(",")],
        Casing ?? CompletionCase.Preserve,
        Sort ?? false,
        Surrounding ?? true
      );
    }
  }

  [AttributeUsage(AttributeTargets.Parameter)]
  public class DynamicCompletionsAttribute(
      ScriptBlock Units,
      CompletionCase? Casing,
      bool? Sort,
      bool? Surrounding
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public IArgumentCompleter Create()
    {
      var invokedUnits = Units.Invoke();
      List<string> unitList = [];

      foreach (var unit in invokedUnits)
      {
        unitList.Add(unit.BaseObject.ToString());
      }

      return new Completer(
        [.. unitList],
        Casing ?? CompletionCase.Preserve,
        Sort ?? false,
        Surrounding ?? true
      );
    }
  }
}
