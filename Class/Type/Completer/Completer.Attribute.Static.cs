using System;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class StaticCompletionsAttribute : BaseCompletionsAttribute
  {
    public readonly string StringifiedDomain;
    public readonly bool Strict;

    private StaticCompletionsAttribute() : base() { }

    public StaticCompletionsAttribute(
      string stringifiedDomain
    ) : this()
    {
      StringifiedDomain = stringifiedDomain;
    }

    public StaticCompletionsAttribute(
      string stringifiedDomain,
      bool strict
    ) : this(stringifiedDomain)
    {
      Strict = strict;
    }

    public StaticCompletionsAttribute(
      string stringifiedDomain,
      bool strict,
      CompletionCase casing
    ) : base(casing)
    {
      StringifiedDomain = stringifiedDomain;
      Strict = strict;
    }

    public override Completer Create()
    {
      return new Completer(
        StringifiedDomain
          .Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries
            | StringSplitOptions.TrimEntries
          ),
        Strict,
        Casing
      );
    }
  }
}
