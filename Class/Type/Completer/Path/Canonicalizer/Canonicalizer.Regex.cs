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
      public static partial Regex DuplicatePathSeparatorRegex();

      [GeneratedRegex(
        @"^(?=(?>[.\\]*)$)"
      )]
      public static partial Regex IsPathDescendantRegex();

      [GeneratedRegex(
        @"^(?=~(?>$|\\))"
      )]
      public static partial Regex IsPathHomeRootedRegex();

      [GeneratedRegex(
        @"^\.(?>$|\\+)"
      )]
      public static partial Regex RemoveRelativeRootRegex();

      [GeneratedRegex(
        @"^~(?>$|\\+)"
      )]
      public static partial Regex RemoveHomeRootRegex();

      [GeneratedRegex(
        @"(?>(?<!^)(?<!:)\\+)$"
      )]
      public static partial Regex RemoveTrailingPathSeparator();
    }
  } // namespace PathCompleter
} // namespace Completer
