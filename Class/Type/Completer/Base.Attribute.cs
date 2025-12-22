using System;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public abstract class BaseCompletionsAttribute<C> : ArgumentCompleterAttribute, IArgumentCompleterFactory where C : BaseCompleter
  {
    public readonly CompletionCase Casing;

    private protected BaseCompletionsAttribute() : base(typeof(C)) { }

    private protected BaseCompletionsAttribute(CompletionCase casing) : this()
    {
      Casing = casing;
    }

    public abstract C Create();
    IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
  }
}
