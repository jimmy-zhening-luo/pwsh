namespace Module.Tab;

public abstract class TabCompleter(
  CompletionCase Casing = default
) : IArgumentCompleter
{
  private static string Escape(
    string text
  ) => text.Contains(
    ' '
  )
    ? string.Concat(
        "'",
        System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(
          text
        ),
        "'"
      )
    : text;

  private static string Unescape(
    string escapedText
  ) => (
    escapedText.Length > 1
    && escapedText.StartsWith(
      '\''
    )
    && escapedText.EndsWith(
      '\''
    )
  )
    ? escapedText[1..^1].Replace(
        "''",
        "'"
      )
    : escapedText;

  public IEnumerable<CompletionResult> CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    System.Management.Automation.Language.CommandAst commandAst,
    IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    GenerateCompletions(
      Unescape(
        wordToComplete
      )
        .Trim()
    )
  );

  private protected abstract IEnumerable<string> GenerateCompletions(
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
      yield return new(
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
