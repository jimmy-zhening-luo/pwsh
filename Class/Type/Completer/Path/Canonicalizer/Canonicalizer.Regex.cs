using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      [GeneratedRegex(
        @"(?<!^)(?>\\\\+)"
      )]
      public static partial Regex DuplicateSeparatorRegex();

      [GeneratedRegex(
        @"^(?=(?>[.\\]*)$)"
      )]
      public static partial Regex IsDescendantOfRegex();
    }
  } // namespace PathCompleter
} // namespace Completer
