namespace Completer.PathCompleter
{
  using System.Text.RegularExpressions;
  public static partial class Canonicalizer
  {
    [GeneratedRegex(
      @"(?<!^)(?>\\\\+)"
    )]
    public static partial Regex DuplicateSeparatorRegex();
  }
}
