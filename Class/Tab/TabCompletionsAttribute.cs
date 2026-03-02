namespace Module.Tab;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
internal abstract class TabCompletionsAttribute(
  CompletionCase Casing = default
) : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Casing { get; init; } = Casing;

  public abstract IArgumentCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
}
