namespace Module.Completer;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
public abstract class BaseCompletionsAttribute<TCompleter, T> : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : BaseCompleter<T>
{
  private protected readonly CompletionCase Casing;

  private protected BaseCompletionsAttribute() : base()
  { }

  private protected BaseCompletionsAttribute(
    CompletionCase casing
  ) : this() => Casing = casing;

  public abstract TCompleter Create();
  IArgumentCompleter IArgumentCompleterFactory
    .Create() => Create();
}
