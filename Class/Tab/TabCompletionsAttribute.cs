namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute<TCompleter>(
  CompletionCase casing = default
) : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : TabCompleter
{
  private protected readonly CompletionCase Casing = casing;

  public abstract TCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
}
