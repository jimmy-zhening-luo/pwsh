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

  public abstract class CompleterBase : IArgumentCompleter
  {
    public readonly required CompletionCase Casing
    {
      get;
      init;
    }

    public CompleterBase() { }

    [SetsRequiredMembers]
    public CompleterBase(CompletionCase? casing)
    {
      Casing = casing ?? CompletionCase.Preserve;
    }

    public IEnumerable<CompletionResult> CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    )
    {
      return WrapArgumentCompletionResult(
        FulfillArgumentCompletion(
          parameterName,
          wordToComplete,
          fakeBoundParameters
        )
      );
    }

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
