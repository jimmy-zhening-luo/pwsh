namespace Module.Tab;

public abstract class TabCompleter(
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
    GenerateCompletion(
      Client.Console.String.UnescapeSingleQuoted(
        wordToComplete
      )
        .Trim()
    )
  );

  private protected abstract IEnumerable<string> GenerateCompletion(string wordToComplete);

  private IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<string> completedStrings)
  {
    foreach (var completedString in completedStrings)
    {
      yield return new(
        Client.Console.String.EscapeSingleQuoted(
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
