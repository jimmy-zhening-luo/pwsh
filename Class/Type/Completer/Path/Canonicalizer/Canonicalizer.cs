using System;
// using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public static string GetHome() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
      );

      public static string Normalize(
        string path,
        string separator = "",
        bool trimLeadingRelative = false,
        bool trimTrailing = false
      )
      {
        string normalPath = DuplicatePathSeparatorRegex().Replace(
          Escaper
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
      }

      public static string AnchorHome(
        string path
      )
      {
        if (!path.StartsWith(HomeChar))
        {
          return path;
        }

        string home = GetHome();
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
      }

      public static string AnchorRelative(
        string path,
        string currentDirectory
      )
      {
        // placeholder
        return currentDirectory + path;
      }
    } // class Canonicalizer
  } // namespace PathCompleter
} // namespace Completer
