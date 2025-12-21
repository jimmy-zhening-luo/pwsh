using System;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public abstract class BaseCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory
  {
    public readonly CompletionCase Casing;

    public BaseCompletionsAttribute() { }

    public BaseCompletionsAttribute(
      CompletionCase casing
    ) : this()
    {
      Casing = casing;
    }

    public abstract BaseCompleter Create();
    IArgumentCompleter IArgumentCompleterFactory.Create() => Create();
  }
}
