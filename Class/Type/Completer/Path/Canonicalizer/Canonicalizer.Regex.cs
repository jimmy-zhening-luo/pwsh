using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      [GeneratedRegex(
        @"(?<!^)\\\\+"
      )]
      private static partial Regex DuplicatePathSeparatorRegex();

      [GeneratedRegex(
        @"^\.(?>$|\\+)"
      )]
      private static partial Regex RemoveRelativeRootRegex();

      [GeneratedRegex(
        @"(?>(?<!^)(?<!:)\\+)$"
      )]
      private static partial Regex RemoveTrailingPathSeparatorRegex();
    }
  } // namespace PathCompleter
} // namespace Completer
