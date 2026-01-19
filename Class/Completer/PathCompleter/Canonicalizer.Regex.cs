namespace Completer.PathCompleter
{
  using GeneratedRegex = System.Text.RegularExpressions.GeneratedRegexAttribute;
  using Regex = System.Text.RegularExpressions.Regex;

  public static partial class Canonicalizer
  {
    [GeneratedRegex(
      @"(?<!^)(?>\\\\+)"
    )]
    public static partial Regex DuplicateSeparatorRegex();
  }
}
