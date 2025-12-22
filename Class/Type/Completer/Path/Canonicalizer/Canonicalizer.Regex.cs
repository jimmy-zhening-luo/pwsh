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
      public static partial Regex DuplicatePathSeparatorRegex();

      [GeneratedRegex(
        @"^(?=(?>[.\\]*)$)"
      )]
      public static partial Regex IsPathDescendantOfRegex();

      [GeneratedRegex(
        @"^\.(?>\\+|$)"
      )]
      public static partial Regex RemoveRelativeRootRegex();

      [GeneratedRegex(
        @"^~(?>\\+|$)"
      )]
      public static partial Regex RemoveHomeRootRegex();
    }
  } // namespace PathCompleter
} // namespace Completer
