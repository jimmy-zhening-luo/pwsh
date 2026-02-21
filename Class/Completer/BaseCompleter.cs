namespace Module.Completer;

public abstract class BaseCompleter(
  CompletionCase Casing = CompletionCase.Preserve
) : IArgumentCompleter
{
  public IEnumerable<CompletionResult> CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    System.Management.Automation.Language.CommandAst commandAst,
    IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    FulfillCompletion(
      Escaper.Unescape(
        wordToComplete
      )
        .Trim()
    )
  );

  private protected abstract IEnumerable<string> FulfillCompletion(
    string wordToComplete
  );

  private protected IEnumerable<CompletionResult> WrapArgumentCompletionResult(
    IEnumerable<string> completedStrings
  )
  {
    foreach (
      var completedString in completedStrings
    )
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
