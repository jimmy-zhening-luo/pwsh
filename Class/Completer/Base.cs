namespace Module.Completer;

using IArgumentCompleter = System.Management.Automation.IArgumentCompleter;
using CompletionResult = System.Management.Automation.CompletionResult;

public abstract class BaseCompleter : IArgumentCompleter
{
  public readonly CompletionCase Casing;

  protected BaseCompleter()
  { }

  protected BaseCompleter(
    CompletionCase casing
  ) : this() => Casing = casing;

  public virtual ICompletionEnumerable CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    CommandAst commandAst,
    IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    FulfillCompletion(
      Unescape(
        wordToComplete
      )
        .Trim()
    )
  );

  public abstract IStringEnumerable FulfillCompletion(
    string wordToComplete
  );

  protected ICompletionEnumerable WrapArgumentCompletionResult(
    IStringEnumerable completedStrings
  )
  {
    foreach (
      string completedString in completedStrings
    )
    {
      yield return new CompletionResult(
        Escape(
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
