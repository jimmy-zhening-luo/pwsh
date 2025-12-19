using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public readonly static char PathSeparatorChar = '\\';
      public readonly static char FriendlyPathSeparatorChar = '/';
      public readonly static char DriveSeparatorChar = ':';
      public readonly static char HomeChar = '~';
      public readonly static string PathSeparator = @"\";
      public readonly static string FriendlyPathSeparator = "/";
      public readonly static string DriveSeparator = ":";
      public readonly static string Home = "~";
      public readonly static string PathSeparatorPattern = @"\\";
      public readonly static string FriendlyPathSeparatorPattern = "/";
      public readonly static string DuplicatePathSeparatorPattern = @"(?<!^)\\\\+";
      public readonly static string IsPathHomeRootedPattern = @"^(?=~(?>$|\\))";
      public readonly static string IsPathRelativelyRootedPattern = @"^(?=\.(?>$|\\))";
      public readonly static string IsPathRelativelyDriveRootedPattern = @"^(?=(?>[^:\\]+):(?>$|[^\\]))";
      public readonly static string IsPathDescendantPattern = @"^(?=(?>[.\\]*)$)";
      public readonly static string HasTrailingPathSeparatorPattern = @"(?<=(?<!^)(?<!:)\\)$";
      public readonly static string RemoveHomeRootPattern = @"^~(?>$|\\+)";
      public readonly static string RemoveRelativeRootPattern = @"^\.(?>$|\\+)";
      public readonly static string RemoveTrailingPathSeparatorPattern = @"(?>(?<!^)(?<!:)\\+)$";
      public readonly static string SubstituteHomeRootPattern = @"^~(?=$|\\)";
      public readonly static string SubstituteRelativeRootPattern = @"^\.(?=$|\\)";

      public static string Normalize(
        string path,
        string separator = "",
        bool trimLeadingRelative = false,
        bool trimTrailing = false
      )
      {
        string normalPath = DuplicatePathSeparatorRegex().Replace(
          Stringifier
            .Unescape(path)
            .Replace(
              FriendlyPathSeparatorChar,
              PathSeparatorChar
            ),
          PathSeparator
        );

        string pretrimmedNormalPath = trimLeadingRelative
          ? RemoveRelativeRootRegex().Replace(
              normalPath,
              string.Empty
            )
          : normalPath;
        string trimmedNormalPath = trimTrailing
          ? RemoveTrailingPathSeparatorRegex().Replace(
              pretrimmedNormalPath,
              string.Empty
            )
          : pretrimmedNormalPath;

        return separator != string.Empty && separator != PathSeparator
          ? trimmedNormalPath.Replace(
              PathSeparator,
              separator
            )
          : trimmedNormalPath;
      } // method Canonicalizer.Normalize

      public static string AnchorHome(
        string path
      )
      {
        if (!path.StartsWith(HomeChar))
        {
          return path;
        }

        string home = Environment.GetFolderPath(
          Environment
            .SpecialFolder
            .UserProfile
        );

        if (path == Home)
        {
          return home;
        }

        if (
          path[1] == PathSeparatorChar
          || path[1] == FriendlyPathSeparatorChar
        )
        {
          return home + path[1..];
        }
        else
        {
          return path;
        }
      } // method Canonicalizer.AnchorHome

      public static string AnchorRelative(
        string path,
        string currentDirectory
      )
      {
        // placeholder
        return currentDirectory + path;
      } // method Canonicalizer.AnchorRelative

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
    } // class Canonicalizer
  } // namespace PathCompleter
} // namespace Completer
