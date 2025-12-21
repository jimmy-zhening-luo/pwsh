using System;
using System.Linq;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class DynamicCompletionsAttribute : CompletionsBaseAttribute
  {
    public readonly ScriptBlock DomainGenerator;
    public readonly bool Strict;

    private DynamicCompletionsAttribute() : base() { }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator
    ) : this()
    {
      DomainGenerator = domainGenerator;
    }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict
    ) : this(domainGenerator)
    {
      Strict = strict;
    }

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict,
      CompletionCase casing
    ) : base(casing)
    {
      DomainGenerator = domainGenerator;
      Strict = strict;
    }

    public override Completer Create()
    {
      return new Completer(
        DomainGenerator
          .Invoke()
          .Select(
            member => member
              .BaseObject
              .ToString()
          ),
        Strict,
        Casing
      );
    }
  }
}
