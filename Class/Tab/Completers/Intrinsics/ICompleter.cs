namespace PowerModule.Tab.Completers.Intrinsics;

internal interface ICompleter : IArgumentCompleter
{
  internal CompletionCase Case
  { get; init; }

  internal CompletionResultType CompletionType
  { get; init; }

  new public IEnumerable<CompletionResult> CompleteArgument(
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
