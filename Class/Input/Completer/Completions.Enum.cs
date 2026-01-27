namespace Module.Input.Completer;

using Type = System.Type;

[AttributeUsage(
  AttributeTargets.Parameter
  | AttributeTargets.Property
  | AttributeTargets.Field
)]
public sealed class EnumCompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  public readonly Type EnumType;

  public readonly bool Strict;

  public EnumCompletionsAttribute(
    Type enumType
  ) : base(
    CompletionCase.Lower
  ) => EnumType = enumType;

  public EnumCompletionsAttribute(
    Type enumType,
    bool strict
  ) : this(
    enumType
  ) => Strict = strict;

  public EnumCompletionsAttribute(
    Type enumType,
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
    Enum.GetNames(
      EnumType
    ),
    Strict,
    Casing
  );
}
