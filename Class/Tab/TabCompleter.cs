namespace Module.Tab;

public abstract partial class TabCompleter(
  CompletionCase Casing = default
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
      Unescape(
        wordToComplete
      )
        .Trim()
    )
  );

  private protected abstract IEnumerable<string> FulfillCompletion(
    string wordToComplete
  );

  private IEnumerable<CompletionResult> WrapArgumentCompletionResult(
    IEnumerable<string> completedStrings
  )
  {
    foreach (
      var completedString in completedStrings
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
