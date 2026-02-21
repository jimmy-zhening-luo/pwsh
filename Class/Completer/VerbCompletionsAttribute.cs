namespace Module.Completer;

public class VerbCompletionsAttribute : CompletionsAttributePrototype<System.Type>
{
  private protected sealed override IEnumerable<string> ResolveDomain()
  {
    var verbInfo = Domain.GetProperty(
      "Verbs",
      System.Reflection.BindingFlags.Static
      | System.Reflection.BindingFlags.Public
    );

    if (verbInfo is null)
    {
      throw new System.ArgumentException(
        "Provided type has no Verbs property.",
        "Domain"
      );
    }

    var verbs = verbInfo.GetValue(
      null
    );

    if (verbs is null)
    {
      throw new System.ArgumentException(
        "Provided Verbs property is null.",
        "Domain.Verbs"
      );
    }

    var domain = verbs as HashSet<string>;

    if (domain is null)
    {
      throw new System.ArgumentException(
        "Provided Verbs property value cannot be cast to HashSet<string>.",
        "Domain.Verbs"
      );
    }

    return domain;
  }
}
