namespace Module.Completer;

public sealed class VerbCompletionsAttribute : BaseCompletionsAttribute<Completer>
{
  private readonly System.Type VerbType;

  private readonly bool Strict;

  public VerbCompletionsAttribute(
    System.Type verbType
  ) : base(
    CompletionCase.Lower
  ) => VerbType = verbType;

  public VerbCompletionsAttribute(
    System.Type verbType,
    bool strict
  ) : this(
    verbType
  ) => Strict = strict;

  public VerbCompletionsAttribute(
    System.Type verbType,
    bool strict,
    CompletionCase casing
  ) : base(
    casing
  ) => (
    VerbType,
    Strict
  ) = (
    verbType,
    strict
  );

  public sealed override Completer Create()
  {
    var verbInfo = VerbType.GetProperty(
      "Verbs",
      System.Reflection.BindingFlags.Static
      | System.Reflection.BindingFlags.Public
    );

    if (verbInfo is null)
    {
      throw new System.ArgumentException(
        "Provided type has no Verbs property.",
        "VerbType"
      );
    }

    return new(
      verbInfo.GetValue(
        null
      ) as HashSet<string>,
      Strict,
      Casing
    );
  }
}
