namespace PowerModule.Tab.Completers.Intrinsics;

interface ICompleter : IArgumentCompleter
{
  sealed private protected record CompletionResultRecord(
    string Result,
    string? DisplayName = default,
    string? Description = default,
    CompletionResultType? CompletionType = default
  );

  internal CompletionResultType CompletionType
  { get; init; }

  internal CompletionCase Case
  { get; init; }

  new IEnumerable<CompletionResult> CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    System.Management.Automation.Language.CommandAst commandAst,
    System.Collections.IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    GenerateCompletion(
      Client.StringInput.UnescapeSingleQuoted(
        wordToComplete
      )
    )
  );
  IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    System.Management.Automation.Language.CommandAst commandAst,
    System.Collections.IDictionary fakeBoundParameters
  ) => CompleteArgument(
    commandName,
    parameterName,
    wordToComplete,
    commandAst,
    fakeBoundParameters
  );

  private protected IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete);

  private IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<CompletionResultRecord> completions)
  {
    foreach (var completion in completions)
    {
      var result = completion.Result;
      var casedResult = Case switch
      {
        CompletionCase.Upper => result.ToUpper(
          Client.StringInput.CurrentCulture
        ),
        CompletionCase.Lower => result.ToLower(
          Client.StringInput.CurrentCulture
        ),
        _ => result,
      };

      yield return new(
        Client.StringInput.EscapeSingleQuoted(casedResult),
        completion.DisplayName ?? casedResult,
        completion.CompletionType ?? CompletionType,
        completion.Description ?? result
      );
    }

    yield break;
  }
}
