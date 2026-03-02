namespace Module.Tab.Completer;

internal class CompletionsAttribute(
  string[] Domain,
  CompletionCase Casing = default
) : CompletionsAttribute<string[]>(
  Domain,
  Casing
)
{
  private protected sealed override IEnumerable<string> ResolveDomain(string[] domain) => domain;
}
internal abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Casing
) : TabCompletionsAttribute(Casing)
{
  public bool Strict { get; init; }

  private protected abstract IEnumerable<string> ResolveDomain(TDomain domain);

  public sealed override Completer Create() => new(
    ResolveDomain(Domain),
    Casing,
    Strict
  );

  internal sealed class Completer(
    IEnumerable<string> Domain,
    CompletionCase Casing,
    bool Strict
  ) : TabCompleter(Casing)
  {
    private protected sealed override IEnumerable<string> GenerateCompletion(string wordToComplete)
    {
      if (wordToComplete is "")
      {
        foreach (var member in Domain)
        {
          yield return member;
        }

        yield break;
      }

      uint matches = default;

      foreach (var member in Domain)
      {
        if (
          member.StartsWith(
            wordToComplete,
            System.StringComparison.OrdinalIgnoreCase
          )
        )
        {
          ++matches;
          yield return member;
        }
      }

      if (
        Strict
        || matches is not 1
      )
      {
        yield break;
      }

      foreach (var member in Domain)
      {
        if (
          member.Length > wordToComplete.Length
          && member.IndexOf(
            wordToComplete,
            1,
            System.StringComparison.OrdinalIgnoreCase
          ) > 0
        )
        {
          ++matches;
          yield return member;
        }
      }

      yield break;
    }
  }
}
