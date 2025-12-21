using System;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class CompletionsAttribute : BaseCompletionsAttribute
  {
    public readonly string StringifiedDomain;
    public readonly bool Strict;

    private CompletionsAttribute() : base() { }

    public CompletionsAttribute(
      string stringifiedDomain
    ) : this()
    {
      StringifiedDomain = stringifiedDomain;
    }

    public CompletionsAttribute(
      string stringifiedDomain,
      bool strict
    ) : this(stringifiedDomain)
    {
      Strict = strict;
    }

    public CompletionsAttribute(
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
