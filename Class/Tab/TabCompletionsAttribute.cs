namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute(CompletionCase Case = default) : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Case { get; init; } = Case;

  public abstract IArgumentCompleter Create();

  internal abstract class TabCompleter(
    CompletionCase Case,
    CompletionResultType CompletionType = CompletionResultType.ParameterValue
  ) : IArgumentCompleter
  {
    private protected record CompletionResultRecord(
      string Result,
      string? DisplayText = default,
      string? Tooltip = default,
      CompletionResultType? CompletionType = default
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

    private protected abstract IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete);

    private IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<CompletionResultRecord> completions)
    {
      foreach (var completion in completions)
      {
        var result = completion.Result;
        var casedResult = Case switch
        {
          CompletionCase.Upper => result.ToUpper(),
          CompletionCase.Lower => result.ToLower(),
          _ => result,
        };

        yield return new(
          Client.Console.String.EscapeSingleQuoted(casedResult),
          completion.DisplayText ?? casedResult,
          completion.CompletionType ?? CompletionType,
          completion.Tooltip ?? result
        );
      }
    }
  }
}
