namespace Module.Tab;

internal class CompletionsAttribute(params string[] Domain) : CompletionsAttribute<string[]>(Domain)
{
  private protected sealed override IEnumerable<string> EnumerateDomain(string[] domain) => domain;
}
internal abstract class CompletionsAttribute<TDomain>(
  TDomain Domain,
  CompletionCase Case = default
) : TabCompletionsAttribute(Case)
{
  public bool Strict { get; init; }

  private protected abstract IEnumerable<string> EnumerateDomain(TDomain domain);

  public sealed override Completer Create() => new(
    EnumerateDomain(Domain),
    Strict,
    Case,
    CompletionType
  );

  internal sealed class Completer(
    IEnumerable<string> Domain,
    bool Strict,
    CompletionCase Case,
    CompletionResultType CompletionType
  ) : TabCompleter(Case, CompletionType)
  {
    private protected sealed override IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete)
    {
      uint index = default;

      if (wordToComplete is "")
      {
        foreach (var member in Domain)
        {
          ++index;
          yield return new(member);
        }

        yield break;
      }

      foreach (var member in Domain)
      {
        if (
          member.StartsWith(
            wordToComplete,
            System.StringComparison.OrdinalIgnoreCase
          )
        )
        {
          ++index;
          yield return new(member);
        }
      }

      if (Strict || index is not 1)
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
          ++index;
          yield return new(member);
        }
      }

      yield break;
    }
  }
}
