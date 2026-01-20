namespace Module.Completer
{

  [AttributeUsage(
    AttributeTargets.Parameter
    | AttributeTargets.Property
    | AttributeTargets.Field
  )]
  public abstract class BaseCompletionsAttribute<T> : ArgumentCompleterAttribute, IArgumentCompleterFactory where T : BaseCompleter
  {
    public readonly CompletionCase Casing;

    protected BaseCompletionsAttribute() : base()
    { }

    protected BaseCompletionsAttribute(
      CompletionCase casing
    ) : this() => Casing = casing;

    public abstract T Create();
    IArgumentCompleter IArgumentCompleterFactory
      .Create() => Create();
  }
}
