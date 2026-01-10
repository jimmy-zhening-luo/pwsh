namespace Completer
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Management.Automation;
  using System.Management.Automation.Language;

  public abstract class BaseCompleter : IArgumentCompleter
  {
    public readonly CompletionCase Casing;

    protected BaseCompleter() { }

    protected BaseCompleter(CompletionCase casing) : this() => Casing = casing;

    public virtual IEnumerable<CompletionResult> CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    ) => WrapArgumentCompletionResult(
      FulfillCompletion(
        Escaper
          .Unescape(wordToComplete)
          .Trim()
      )
    );

    public abstract IEnumerable<string> FulfillCompletion(string wordToComplete);

    protected IEnumerable<CompletionResult> WrapArgumentCompletionResult(
      IEnumerable<string> completedStrings
    )
    {
      foreach (string completedString in completedStrings)
      {
        yield return new CompletionResult(
          Escaper.Escape(
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
  }
}
