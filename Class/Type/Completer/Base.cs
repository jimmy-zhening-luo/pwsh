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
  } // enum CompletionCase

  [AttributeUsage(AttributeTargets.Parameter)]
  public abstract class CompletionsBaseAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public readonly CompletionCase Casing;

    public CompletionsBaseAttribute() { }

    public CompletionsBaseAttribute(
      CompletionCase casing
    ) : this()
    {
      Casing = casing;
    }

    public abstract CompleterBase Create();
    IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
  }

  public abstract class CompleterBase : IArgumentCompleter
  {
    public readonly CompletionCase Casing;

    public CompleterBase() { }

    public CompleterBase(
      CompletionCase casing
    ) : this()
    {
      Casing = casing;
    }

    public IEnumerable<CompletionResult> CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    ) => WrapArgumentCompletionResult(
      FulfillArgumentCompletion(
        parameterName,
        wordToComplete,
        fakeBoundParameters
      )
    );

    protected abstract IEnumerable<string> FulfillArgumentCompletion(
      string parameterName,
      string wordToComplete,
      IDictionary fakeBoundParameters
    );

    private IEnumerable<CompletionResult> WrapArgumentCompletionResult(
      IEnumerable<string> completedStrings
    )
    {
      foreach (string completedString in completedStrings)
      {
        yield return new CompletionResult(
          Stringifier.Escape(
            Casing switch
            {
              CompletionCase.Upper => completedString.ToUpper(),
              CompletionCase.Lower => completedString.ToLower(),
              _ => completedString,
            }
          )
        );
      }
    }
  } // class CompleterBase
} // namespace Completer
