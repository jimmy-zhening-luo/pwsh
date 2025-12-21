using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class DynamicCompletionsAttribute : CompletionsBaseAttribute
  {
    public readonly ScriptBlock DomainGenerator;
    public readonly bool Strict;

    private DynamicCompletionsAttribute() : base() { }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator
    ) : this()
    {
      DomainGenerator = domainGenerator;
    }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict
    ) : this(domainGenerator)
    {
      Strict = strict;
    }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict,
      CompletionCase casing
    ) : base(casing)
    {
      DomainGenerator = domainGenerator;
      Strict = strict;
    }

    public override Completer Create()
    {
      return new Completer(
        DomainGenerator
          .Invoke()
          .Select(
            member => member
              .BaseObject
              .ToString()
          ),
        Strict,
        Casing
      );
    }
  } // class DynamicCompletionsAttribute

  [AttributeUsage(AttributeTargets.Parameter)]
  public class StaticCompletionsAttribute : CompletionsBaseAttribute
  {
    public readonly string StringifiedDomain;
    public readonly bool Strict;

    private StaticCompletionsAttribute() : base() { }

    public StaticCompletionsAttribute(
      string stringifiedDomain
    ) : this()
    {
      StringifiedDomain = stringifiedDomain;
    }

    public StaticCompletionsAttribute(
      string stringifiedDomain,
      bool strict
    ) : this(stringifiedDomain)
    {
      Strict = strict;
    }

    public StaticCompletionsAttribute(
      string stringifiedDomain,
      bool strict,
      CompletionCase casing
    ) : base(casing)
    {
      StringifiedDomain = stringifiedDomain;
      Strict = strict;
    }

    public override Completer Create()
    {
      return new Completer(
        StringifiedDomain
          .Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries
            | StringSplitOptions.TrimEntries
          ),
        Strict,
        Casing
      );
    }
  } // class StaticCompletionsAttribute

  public class Completer : CompleterBase
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
  } // class Completer
} // namespace Completer
