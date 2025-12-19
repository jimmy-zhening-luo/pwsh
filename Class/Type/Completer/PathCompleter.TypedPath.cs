using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static class TypedPath
    {
      public readonly static char PathSeparatorChar = '\\';
      public readonly static char FriendlyPathSeparatorChar = '/';
      public readonly static string PathSeparator = @"\";
      public readonly static string FriendlyPathSeparator = "/";
      public readonly static string PathSeparatorPattern = @"\\";
      public readonly static string FriendlyPathSeparatorPattern = "/";
      public readonly static string DuplicatePathSeparatorPattern = @"(?<!^)\\\\+";
      public readonly static string IsPathTildeRootedPattern = @"^(?=~(?>$|\\))";
      public readonly static string IsPathRelativelyRootedPattern = @"^(?=\.(?>$|\\))";
      public readonly static string IsPathDescendantPattern = @"^(?=(?>[.\\]*)$)";
      public readonly static string HasTrailingPathSeparatorPattern = @"(?<=(?<!^)(?<!:)\\)$";
      public readonly static string RemoveTildeRootPattern = @"^~(?>$|\\+)";
      public readonly static string RemoveRelativeRootPattern = @"^\.(?>$|\\+)";
      public readonly static string RemoveTrailingPathSeparatorPattern = @"(?>(?<!^)(?<!:)\\+)$";
      public readonly static string SubstituteTildeRootPattern = @"^~(?=$|\\)";
      public readonly static string SubstituteRelativeRootPattern = @"^\.(?=$|\\)";

      public static string Normalize(
        string path,
        string separator = "",
        bool trimLeadingRelative = false,
        bool trimTrailing = false
      )
      {
        string normalPath = Regex.Replace(
          Typed
            .Unescape(path)
            .Replace(
              FriendlyPathSeparatorChar,
              PathSeparatorChar
            ),
          DuplicatePathSeparatorPattern,
          PathSeparator
        );

        string pretrimmedNormalPath = trimLeadingRelative
          ? Regex.Replace(
              normalPath,
              RemoveRelativeRootPattern,
              string.Empty
            )
          : normalPath;
        string trimmedNormalPath = trimTrailing
          ? Regex.Replace(
              pretrimmedNormalPath,
              RemoveTrailingPathSeparatorPattern,
              string.Empty
            )
          : pretrimmedNormalPath;

        return separator != string.Empty && separator != PathSeparator
          ? trimmedNormalPath.Replace(
              PathSeparator,
              separator
            )
          : trimmedNormalPath;
      }
    }
  }
}
