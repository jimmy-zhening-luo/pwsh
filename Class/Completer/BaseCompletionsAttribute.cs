namespace Module.Completer;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
public abstract class BaseCompletionsAttribute<TCompleter>(
  CompletionCase casing = CompletionCase.Preserve
) : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : BaseCompleter
{
  private protected readonly CompletionCase Casing = casing;

  public abstract TCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory
    .Create() => Create();
}
