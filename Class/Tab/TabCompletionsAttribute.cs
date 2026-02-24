namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
public abstract class TabCompletionsAttribute<TCompleter>(
  CompletionCase casing = CompletionCase.Preserve
) : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : TabCompleter
{
  private protected readonly CompletionCase Casing = casing;

  public abstract TCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory
    .Create() => Create();
}
