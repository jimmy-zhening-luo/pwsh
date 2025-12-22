using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public static bool IsPathDescendant(
        string path,
        string location
      ) => IsPathDescendantRegex().IsMatch(
        Path.GetRelativePath(
          path,
          location
        )
      );

      public static string Home() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
      );

      public static bool IsPathHomeRooted(string path) => IsPathHomeRootedRegex().IsMatch(path);

      public static string RemoveHomeRoot(string path) => RemoveHomeRootRegex().Replace(
        path,
        string.Empty
      );

      public static string Normalize(
        string path,
        bool trimLeadingRelative = false,
        bool trimTrailing = false
      )
      {
        string normalPath = DuplicatePathSeparatorRegex().Replace(
          Escaper
            .Unescape(path)
            .Replace(
              '/',
              '\\'
            ),
          @"\"
        );
        string pretrimmedNormalPath = trimLeadingRelative
          ? RemoveRelativeRootRegex().Replace(
              normalPath,
              string.Empty
            )
          : normalPath;

        return trimTrailing
          ? RemoveTrailingPathSeparator().Replace(
              pretrimmedNormalPath,
              string.Empty
            )
          : pretrimmedNormalPath;
      }

      public static string AnchorHome(
        string path
      )
      {
        if (!path.StartsWith('~'))
        {
          return path;
        }

        string home = Home();
        if (path == "~")
        {
          return home;
        }

        if (
          path[1] == '\\'
          || path[1] == '/'
        )
        {
          return home + path[1..];
        }
        else
        {
          return path;
        }
      }
    }
  } // namespace PathCompleter
} // namespace Completer
