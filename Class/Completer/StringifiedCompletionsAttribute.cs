namespace Module.Completer;

public sealed class StringifiedCompletionsAttribute : CompletionsAttributePrototype<string>
{
  private protected sealed override IEnumerable<string> ResolveDomain() => Domain.Split(
    ',',
    System.StringSplitOptions.RemoveEmptyEntries
    | System.StringSplitOptions.TrimEntries
  );
}
