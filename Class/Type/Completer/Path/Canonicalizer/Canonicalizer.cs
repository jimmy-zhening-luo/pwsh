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
        bool preserveTrailingSeparator = false
      )
      {
        string normalPath = RemoveRelativeRootRegex().Replace(
          DuplicatePathSeparatorRegex().Replace(
            Escaper
              .Unescape(path)
              .Replace('/', '\\'),
            @"\"
          ),
          string.Empty
        );

        return preserveTrailingSeparator
          ? normalPath
          : RemoveTrailingPathSeparator().Replace(
              normalPath,
              string.Empty
            );
      }

      public static string Denormalize(
        string path,
        string location = "",
        string subpath = ""
      ) => Path
        .Join(
          location,
          path,
          subpath
        )
        .Replace('\\', '/');

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
