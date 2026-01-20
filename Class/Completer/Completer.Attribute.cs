namespace Module.Completer
{
  using static System.StringSplitOptions;

  [AttributeUsage(
    Parameter
    | Property
    | Field
  )]
  public class CompletionsAttribute : BaseCompletionsAttribute<Completer>
  {
    public readonly string StringifiedDomain;

    public readonly bool Strict;

    public CompletionsAttribute(
      string stringifiedDomain
    ) : base() => StringifiedDomain = stringifiedDomain;

    public CompletionsAttribute(
      string stringifiedDomain,
      bool strict
    ) : this(
      stringifiedDomain
    ) => Strict = strict;

    public CompletionsAttribute(
      string stringifiedDomain,
      bool strict,
      CompletionCase casing
    ) : base(
      casing
    ) => (
      StringifiedDomain,
      Strict
    ) = (
      stringifiedDomain,
      strict
    );

    public override Completer Create() => new(
      StringifiedDomain
        .Split(
          ',',
          RemoveEmptyEntries
          | TrimEntries
        ),
      Strict,
      Casing
    );
  }
}
