namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute<TCompleter>(
  CompletionCase Casing = default
) : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : TabCompleter
{
  public CompletionCase Casing { get; init; } = Casing;

  public abstract TCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
}
