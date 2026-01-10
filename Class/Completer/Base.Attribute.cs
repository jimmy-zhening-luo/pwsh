namespace Completer
{
  using System;
  using System.Management.Automation;

  [AttributeUsage(
    AttributeTargets.Parameter
    | AttributeTargets.Property
    | AttributeTargets.Field
  )]
  public abstract class BaseCompletionsAttribute<TCompleter> : ArgumentCompleterAttribute, IArgumentCompleterFactory where TCompleter : BaseCompleter
  {
    public readonly CompletionCase Casing;

    protected BaseCompletionsAttribute() : base() { }

    protected BaseCompletionsAttribute(CompletionCase casing) : this() => Casing = casing;

    public abstract TCompleter Create();
    IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
  }
}
