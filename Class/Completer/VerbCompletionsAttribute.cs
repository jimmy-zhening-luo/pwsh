namespace Module.Completer;

public class VerbCompletionsAttribute : CompletionsAttributePrototype<System.Type>
{
  public VerbCompletionsAttribute(
    System.Type verbType
  ) : base(
    verbType
  )
  { }

  public VerbCompletionsAttribute(
    System.Type verbType,
    bool strict
  ) : base(
    verbType,
    strict
  )
  { }

  private protected sealed override IEnumerable<string> ResolveDomain(
    System.Type verbType
  )
  {
    var verbs = verbType
      .GetProperty(
        "Verbs",
        System.Reflection.BindingFlags.Static
        | System.Reflection.BindingFlags.Public
      )?.GetValue(
        null
      );

    if (
      verbs is not HashSet<string> domain
    )
    {
      throw new System.ArgumentException(
        "Provided type has no Verbs property that casts to HashSet<string>.",
        nameof(verbType)
      );
    }

    return domain;
  }
}
