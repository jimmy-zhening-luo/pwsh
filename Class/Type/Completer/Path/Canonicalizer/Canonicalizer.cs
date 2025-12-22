using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public static string Home() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
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

      public static string Denormalize(string path) => path.Replace(
        '\\',
        '/'
      );

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

      public static bool IsPathHomeRooted(string path) => IsPathHomeRootedRegex().IsMatch(path);

      public static bool IsPathDescendantOf(
        string path,
        string location
      ) => IsPathDescendantOfRegex().IsMatch(
        Path.GetRelativePath(
          path,
          location
        )
      );
  
      public static string RemoveHomeRoot(string path) => RemoveHomeRootRegex().Replace(
        path,
        string.Empty
      );
    }
  } // namespace PathCompleter
} // namespace Completer
