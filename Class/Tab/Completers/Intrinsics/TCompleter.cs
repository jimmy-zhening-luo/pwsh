namespace PowerModule.Tab.Completers.Intrinsics;

abstract internal class TCompleter(
  CompletionCase Case,
  CompletionResultType CompletionType
) : IArgumentCompleter
{
  sealed private protected record CompletionResultRecord(
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
      Client.String.UnescapeSingleQuoted(
        wordToComplete
      )
    )
  );

  abstract private protected IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete);

  private IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<CompletionResultRecord> completions)
  {
    foreach (var completion in completions)
    {
      var result = completion.Result;
      var casedResult = Case switch
      {
        CompletionCase.Upper => result.ToUpper(
          Client.String.CurrentCulture
        ),
        CompletionCase.Lower => result.ToLower(
          Client.String.CurrentCulture
        ),
        _ => result,
      };

      yield return new(
        Client.String.EscapeSingleQuoted(casedResult),
        completion.DisplayName ?? casedResult,
        completion.CompletionType ?? CompletionType,
        completion.Description ?? result
      );
    }

    yield break;
  }
}
