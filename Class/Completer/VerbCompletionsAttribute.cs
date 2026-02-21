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

    var verbs = verbInfo.GetValue(
      null
    );

    if (verbs is null)
    {
      throw new System.ArgumentException(
        "Provided Verbs property is null.",
        "VerbType.Verbs"
      );
    }

    var domain = verbs as HashSet<string>;

    if (domain is null)
    {
      throw new System.ArgumentException(
        "Provided Verbs property value cannot be cast to HashSet<string>.",
        "VerbType.Verbs"
      );
    }

    return new(
      domain,
      Strict,
      Casing
    );
  }
}
