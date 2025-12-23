using System;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class CompletionsAttribute : BaseCompletionsAttribute<Completer>
  {
    public readonly string StringifiedDomain;
    public readonly bool Strict;

    private CompletionsAttribute() : base() { }

    public CompletionsAttribute(string stringifiedDomain) : this() => StringifiedDomain = stringifiedDomain;

    public CompletionsAttribute(
      string stringifiedDomain,
      bool strict
    ) : this(stringifiedDomain) => Strict = strict;

    public CompletionsAttribute(
      string stringifiedDomain,
      bool strict,
      CompletionCase casing
    ) : base(casing) => this(
      stringifiedDomain,
      strict
    );

    public override Completer Create() => new(
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
