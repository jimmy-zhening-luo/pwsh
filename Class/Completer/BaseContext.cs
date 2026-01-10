namespace Completer
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Management.Automation;
  using System.Management.Automation.Language;

  public abstract class BaseContextCompleter : BaseCompleter
  {
    protected BaseContextCompleter() : base() { }

    protected BaseContextCompleter(CompletionCase casing) : base(casing) { }

    public override IEnumerable<CompletionResult> CompleteArgument(
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

    public abstract IEnumerable<string> FulfillCompletion(
      string wordToComplete,
      CommandAst commandAst,
      IDictionary fakeBoundParameters
    );
  }
}
