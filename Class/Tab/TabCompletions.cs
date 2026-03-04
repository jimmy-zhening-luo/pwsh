namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute(
  CompletionCase Case = default,
  CompletionResultType CompletionType = CompletionResultType.ParameterValue
) : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Case { get; init; } = Case;

  public CompletionResultType CompletionType { get; init; } = CompletionType;

  public abstract IArgumentCompleter Create();

  internal abstract class TabCompleter(
    CompletionCase Case,
    CompletionResultType CompletionType
  ) : IArgumentCompleter
  {
    private protected record CompletionResultRecord(
      string Result,
      string? DisplayName = default,
      string? Description = default,
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
          completion.DisplayName ?? casedResult,
          completion.CompletionType ?? CompletionType,
          completion.Description ?? result
        );
      }
      yield break;
    }
  }
}
