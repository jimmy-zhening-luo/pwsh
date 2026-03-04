namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Case { get; init; } = default;

  public abstract IArgumentCompleter Create();

  internal abstract class TabCompleter(CompletionCase Case) : IArgumentCompleter
  {
    private protected record CompletionResultRecord(
      string Result,
      string? DisplayText = default,
      string? Tooltip = default,
      CompletionResultType CompletionType = CompletionResultType.ParameterValue
    );

    public IEnumerable<CompletionResult> CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      System.Management.Automation.Language.CommandAst commandAst,
      System.Collections.IDictionary fakeBoundParameters
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
            Case switch
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
