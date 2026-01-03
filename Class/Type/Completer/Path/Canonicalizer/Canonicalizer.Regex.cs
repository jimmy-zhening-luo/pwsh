using System.Text.RegularExpressions;

namespace Completer.PathCompleter
{
  public static partial class Canonicalizer
  {
    [GeneratedRegex(
      @"(?<!^)(?>\\\\+)"
    )]
    public static partial Regex DuplicateSeparatorRegex();
  }
}
