namespace Completer
{
  public abstract class BaseContextCompleter : BaseCompleter
  {
    protected BaseContextCompleter() : base()
    { }

    protected BaseContextCompleter(
      CompletionCase casing
    ) : base(casing)
    { }

    public override ICompletionEnumerable CompleteArgument(
      string commandName,
      string parameterName,
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    ) => WrapArgumentCompletionResult(
      FulfillCompletion(
        Escaper
          .Unescape(wordToComplete)
          .Trim(),
        commandAst,
        fakeBoundParameters
      )
    );

    public abstract IStringEnumerable FulfillCompletion(
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    );
  }
}
