namespace Module.Completer;

using AttributeTargets = System.AttributeTargets;

[System.AttributeUsage(
  AttributeTargets.Property
  | AttributeTargets.Field
)]
public abstract class BaseCompletionsAttribute<T> : ArgumentCompleterAttribute, IArgumentCompleterFactory where T : BaseCompleter
{
  private protected readonly CompletionCase Casing;

  private protected BaseCompletionsAttribute() : base()
  { }

  private protected BaseCompletionsAttribute(
    CompletionCase casing
  ) : this() => Casing = casing;

  public abstract T Create();
  IArgumentCompleter IArgumentCompleterFactory
    .Create() => Create();
}
