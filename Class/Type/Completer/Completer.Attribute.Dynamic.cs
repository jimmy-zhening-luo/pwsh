using System;
using System.Linq;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(
    AttributeTargets.Parameter
    | AttributeTargets.Property
  )]
  public class DynamicCompletionsAttribute : BaseCompletionsAttribute<Completer>
  {
    public readonly ScriptBlock DomainGenerator;
    public readonly bool Strict;

    public DynamicCompletionsAttribute(ScriptBlock domainGenerator) : base() => DomainGenerator = domainGenerator;

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict
    ) : this(domainGenerator) => Strict = strict;

    public DynamicCompletionsAttribute(
      ScriptBlock domainGenerator,
      bool strict,
      CompletionCase casing
    ) : base(casing) => (DomainGenerator, Strict) = (
      domainGenerator,
      strict
    );

    public override Completer Create() => new(
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
