namespace PowerModule.Tab.Completers.Intrinsics;

internal interface ICompleter(
  CompletionCase Case,
  CompletionResultType CompletionType
) : IArgumentCompleter
{
  private protected CompletionCase Case
  { get; set; }

  private protected CompletionResultType CompletionType
  { get; set; }

  private protected IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete);

  virtual private protected IEnumerable<CompletionResult> WrapArgumentCompletionResult(IEnumerable<CompletionResultRecord> completions)
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
