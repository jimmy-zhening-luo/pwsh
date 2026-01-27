namespace Module.Input.Completer;

public abstract class BaseCompleter : IArgumentCompleter
{
  public readonly CompletionCase Casing;

  private protected BaseCompleter()
  { }

  private protected BaseCompleter(
    CompletionCase casing
  ) : this() => Casing = casing;

  public IEnumerable<CompletionResult> CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    Language.CommandAst commandAst,
    IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    FulfillCompletion(
      Unescape(
        wordToComplete
      )
        .Trim()
    )
  );

  public abstract IEnumerable<string> FulfillCompletion(
    string wordToComplete
  );

  private protected IEnumerable<CompletionResult> WrapArgumentCompletionResult(
    IEnumerable<string> completedStrings
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
