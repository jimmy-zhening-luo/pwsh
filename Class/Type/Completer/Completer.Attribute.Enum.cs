using System;
using System.Linq;
using System.Management.Automation;

namespace Completer
{
  [AttributeUsage(AttributeTargets.Parameter)]
  public class EnumCompletionsAttribute : BaseCompletionsAttribute<Completer>
  {
    public readonly string[] EnumMembers;
    public readonly bool Strict;

    private EnumCompletionsAttribute() : base(CompletionCase.Lower) { }

    public EnumCompletionsAttribute(in Type enumType) : this() => EnumMembers = Enum.GetNames(enumType);

    public EnumCompletionsAttribute(
      in Type enumType,
      bool strict
    ) : this(in enumType) => Strict = strict;

    public EnumCompletionsAttribute(
      in Type enumType,
      bool strict,
      CompletionCase casing
    ) : base(casing) => (EnumMembers, Strict) = (
      Enum.GetNames(enumType),
      strict
    );

    public override Completer Create() => new(
      EnumMembers,
      Strict,
      Casing
    );
  }
}
