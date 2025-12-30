using System;

namespace Completer
{
  [AttributeUsage(
    AttributeTargets.Field
    | AttributeTargets.Property
    | AttributeTargets.Parameter
  )]
  public class EnumCompletionsAttribute : BaseCompletionsAttribute<Completer>
  {
    public readonly Type EnumType;
    public readonly bool Strict;

    public EnumCompletionsAttribute(Type enumType) : base() => EnumType = enumType;

    public EnumCompletionsAttribute(
      Type enumType,
      bool strict
    ) : this(enumType) => Strict = strict;

    public EnumCompletionsAttribute(
      Type enumType,
      bool strict,
      CompletionCase casing
    ) : base(casing) => (EnumType, Strict) = (
      enumType,
      strict
    );

    public override Completer Create() => new(
      Enum.GetNames(EnumType),
      Strict,
      Casing
    );
  }
}
