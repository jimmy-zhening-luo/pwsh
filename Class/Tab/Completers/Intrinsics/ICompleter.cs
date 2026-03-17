namespace PowerModule.Tab.Completers.Intrinsics;

using CommandAst = System.Management.Automation.Language.CommandAst;

interface ICompleter : IArgumentCompleter
{
  sealed private protected record Completion(
    string Result,
    string? Description = default
  );

  CompletionResultType CompletionType
  { get; init; }

  CompletionCase Casing
  { get; init; }

  new IEnumerable<CompletionResult> CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    CommandAst commandAst,
    System.Collections.IDictionary fakeBoundParameters
  ) => WrapArgumentCompletionResult(
    GenerateCompletion(
      Client.StringInput.Unescape(
        wordToComplete
      )
    )
  );
  IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(
    string commandName,
    string parameterName,
    string wordToComplete,
    CommandAst commandAst,
    System.Collections.IDictionary fakeBoundParameters
  ) => CompleteArgument(
    commandName,
    parameterName,
    wordToComplete,
    commandAst,
    fakeBoundParameters
  );

  private protected IEnumerable<Completion> GenerateCompletion(string wordToComplete);

  private IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<Completion> completions)
  {
    foreach (var completion in completions)
    {
      var result = completion.Result;
      var casedResult = Casing switch
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
        Client.StringInput.EscapeSingleQuoted(
          casedResult
        ),
        casedResult,
        CompletionType,
        completion.Description ?? result
      );
    }

    yield break;
  }
}
