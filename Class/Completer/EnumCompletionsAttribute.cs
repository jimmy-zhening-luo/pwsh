namespace Module.Completer;

public sealed class EnumCompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  private readonly System.Type EnumType;

  private readonly bool Strict;

  public EnumCompletionsAttribute(
    System.Type enumType
  ) : base(
    CompletionCase.Lower
  ) => EnumType = enumType;

  public EnumCompletionsAttribute(
    System.Type enumType,
    bool strict
  ) : this(
    enumType
  ) => Strict = strict;

  public EnumCompletionsAttribute(
    System.Type enumType,
    bool strict,
    CompletionCase casing
  ) : base(
    casing
  ) => (
    EnumType,
    Strict
  ) = (
    enumType,
    strict
  );

  public sealed override Completer Create() => new(
    System.Enum.GetNames(
      EnumType
    ),
    Strict,
    Casing
  );
}
